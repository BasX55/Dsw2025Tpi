using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
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
        
        var order = new Order
        {
            Id = Guid.NewGuid(),
            //CustomerID = request.CustomerId,
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
        await _repository.Add(order);
        return new OrderModel.Response(order.Id);
    }
}
