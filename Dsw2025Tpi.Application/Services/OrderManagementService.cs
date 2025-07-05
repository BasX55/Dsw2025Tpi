using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services;

public class OrderManagementService
{
    private readonly IRepository _repository;

    public OrderManagementService(IRepository repository)
    {
        _repository = repository;
    }
    public async Task<OrderModel.Response> AddOrder(OrderModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.ShippingAddress) ||
            string.IsNullOrWhiteSpace(request.BillingAddress))
        {
            throw new ArgumentException("No puede estar vacía el Shipping Address ni el BillingAddress");
        }
        if (request.OrderItems == null || !request.OrderItems.Any())
        {
            throw new ArgumentException("Debe incluir al menos un producto en la orden");
        }
        if (request.OrderItems.Any(p => p.Quantity <= 0 || p.UnitPrice <= 0))
        {
            throw new ArgumentException("Los valores de cantidad y precio unitario deben ser mayores a cero");
        }
        if (request.OrderItems.Any(p => p.ProductId == Guid.Empty))
        {
            throw new ArgumentException("El ProductId no puede ser un Guid vacío");
        }
        if (request.CustomerId == Guid.Empty)
        {
            throw new ArgumentException("El CustomerId no puede ser un Guid vacío");
        }



        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerID = request.CustomerId,
            Date = DateTime.UtcNow,
            
            
            OrderItems = request.OrderItems.Select(p => new OrderItem
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice,
                Description = p.Description
            }).ToList(),

            ShippingAddress = request.ShippingAddress,
            BillingAddress = request.BillingAddress,
        };
        order.Status = 0;

        //modificar stock de los productos
        foreach (var item in order.OrderItems)
        {
            // Specify the type explicitly for the generic method GetById<T>
            var product = await _repository.GetById<Product>(item.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"El producto con ID {item.ProductId} no existe.");
            }
            if (product.StockQuantity < item.Quantity)
            {
                throw new ArgumentException($"No hay suficiente stock para el producto {product.Name}. Stock disponible: {product.StockQuantity}, cantidad solicitada: {item.Quantity}.");
            }
            //product.StockQuantity -= item.Quantity;
            product.DecreaseStock(item.Quantity);
            await _repository.Update<Product>(product);
           

        }

        await _repository.Add(order);
        return new OrderModel.Response(order.Id, order.Date, order.CustomerID, 
            order.ShippingAddress, order.BillingAddress, order.Status.ToString(), 
            order.TotalAmount, order.OrderItems.Select(oi => new OrderModel.OrderItemResponse(
                oi.ProductId, oi.Quantity, oi.UnitPrice, oi.Description)).ToList());
    }

    public async Task<OrderModel.Response?> GetOrderById(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("El ID de la orden no puede ser un Guid vacío");
        }
        var order = await _repository.GetById<Order>(id);
        if (order == null)
        {
            return null;
        }
        
        var orderItems = await _repository.GetFiltered<OrderItem>(oi => oi.OrderId == id, "Product");

        return new OrderModel.Response(
            order.Id,
            order.Date,
            order.CustomerID,
            order.ShippingAddress,
            order.BillingAddress,
            order.Status.ToString(),
            order.TotalAmount,
            orderItems.Select(oi => new OrderModel.OrderItemResponse(
                oi.ProductId,
                oi.Quantity,
                oi.UnitPrice,
                oi.Description)).ToList()
        );
    }
    public async Task<IEnumerable<OrderModel.Response>> GetAllOrders()
    {
        var orders = await _repository.GetAll<Order>("OrderItems");

        return orders.Select(order => new OrderModel.Response(
            order.Id,
            order.Date,
            order.CustomerID,
            order.ShippingAddress,
            order.BillingAddress,
            order.Status.ToString(),
            order.TotalAmount,
            order.OrderItems.Select(oi => new OrderModel.OrderItemResponse(
                oi.ProductId,
                oi.Quantity,
                oi.UnitPrice,
                oi.Description)).ToList()
        ));
    }

    public async Task<OrderModel.ResponseId> UpdateOrderStatus(Guid id, string status)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("El ID de la orden no puede ser un Guid vacío");
        }
        var order = await _repository.GetById<Order>(id);
        if (order == null)
        {
            throw new ArgumentException($"No existe una orden con el ID {id}");
        }
        if (!Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
        {
            throw new ArgumentException($"El estado '{status}' no es válido.");
        }

        order.Status = parsedStatus; // o guardás el enum directamente si tu modelo lo permite
        
        await _repository.Update(order);

        return new OrderModel.ResponseId(order.Id);
    }

   
}
