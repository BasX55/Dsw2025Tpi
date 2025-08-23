using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dsw2025Tpi.Application.Services;

public class OrderManagementService
{
    private readonly IRepository _repository;
    private readonly ILogger<OrderManagementService> _logger;

    public OrderManagementService(IRepository repository, ILogger<OrderManagementService> logger)
    {
        _logger = logger;
        _repository = repository;
    }
    public async Task<OrderModel.Response> AddOrder(OrderModel.Request request)
    {
        _logger.LogInformation("Iniciando el proceso de creación de una nueva orden");
        if (request.CustomerId == Guid.Empty)
            throw new ArgumentException("El CustomerId no puede ser un Guid vacío", nameof(request.CustomerId));
        var customer = await _repository.GetById<Customer>(request.CustomerId) ??
            throw new EntityNotFoundException($"No se encontró el cliente con ID {request.CustomerId}");

        if (string.IsNullOrWhiteSpace(request.ShippingAddress) ||
            string.IsNullOrWhiteSpace(request.BillingAddress))
            throw new ArgumentException("No puede estar vacía el Shipping Address ni el BillingAddress");

        if (request.OrderItems == null)
            throw new ArgumentNullException(nameof(request.OrderItems), "La lista de productos no puede ser nula");

        if (!request.OrderItems.Any())
            throw new ArgumentException("Debe incluir al menos un producto en la orden", nameof(request.OrderItems));

        if (request.OrderItems.Any(p => p.Quantity < 0))
            throw new ArgumentException("La cantidad de los productos no puede ser menor a cero");

        if (request.OrderItems.Any(p => p.ProductId == Guid.Empty))
            throw new ArgumentException("El ProductId no puede ser un Guid vacío");

        if (request.CustomerId == Guid.Empty)
            throw new ArgumentException("El CustomerId no puede ser un Guid vacío");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerID = request.CustomerId,
            Date = DateTime.UtcNow,

            OrderItems = request.OrderItems.Select(p => new OrderItem
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                Description = string.Empty
            }).ToList(),

            ShippingAddress = request.ShippingAddress,
            BillingAddress = request.BillingAddress,
        };

        order.Status = 0;

        //modificar stock de los productos
        foreach (var item in order.OrderItems)
        {

            var product = await _repository.GetById<Product>(item.ProductId) ?? 
                throw new EntityNotFoundException($"El producto con ID {item.ProductId} no existe.");

            if (product.StockQuantity < item.Quantity)
                throw new InsufficientStockException($"No hay suficiente stock para el producto {product.Name}. Stock disponible: {product.StockQuantity}, cantidad solicitada: {item.Quantity}.");
            
            if(product.IsActive == false)
                throw new InactiveProductException($"El producto {product.Name} no está activo y no puede ser incluido en la orden.");
            
            if (product.CurrentUnitPrice <= 0)
                throw new InvalidProductPriceException($"El precio unitario del producto {product.Name} debe ser mayor a cero.");
            
            if (item.Quantity < 0)
                throw new ArgumentException($"La cantidad del producto {product.Name} no debe ser menor a cero.");
            
            product.DecreaseStock(item.Quantity);
            item.UnitPrice = product.CurrentUnitPrice;
            item.Description = product.Description;

            await _repository.Update<Product>(product);
            _logger.LogInformation($"Producto {product.Name} actualizado. Stock restante: {product.StockQuantity}");
        }

        await _repository.Add(order);
        _logger.LogInformation($"Orden {order.Id} creada exitosamente para el cliente {customer.Name}");
        return new OrderModel.Response(
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
                oi.Description
            )).ToList()
        );
    }
    public async Task<OrderModel.Response?> GetOrderById(Guid id)
    {
        _logger.LogInformation($"Buscando la orden con ID {id}");
        if (id == Guid.Empty)
            throw new ArgumentException("El ID de la orden no puede ser un Guid vacío");
        
        var order = await _repository.GetById<Order>(id) ?? 
            throw new EntityNotFoundException($"No se encontró la orden con ID {id}");

        var orderItems = await _repository.GetFiltered<OrderItem>(oi => oi.OrderId == id, "Product") ??
            throw new EntityNotFoundException($"No se encontraron items de la orden con ID {id}");
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
                oi.Description
            )).ToList()
        );
    }
    


    
    public async Task<IEnumerable<OrderModel.Response>> GetAllOrders(string? status, Guid? customerId, int? pageNumber, int? pageSize)
    {
        _logger.LogInformation("Obteniendo todas las órdenes con los filtros proporcionados");
        var orders = await _repository.GetAll<Order>("OrderItems") ??
            throw new EntityNotFoundException("No existen órdenes registradas");
        
        
        if (status != null && customerId.HasValue)
        {
            if (!Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))   
                throw new ArgumentException($"El estado '{status}' no es válido.");

            orders = await _repository.GetFiltered<Order>(
                o => o.Status == parsedStatus && o.CustomerID == customerId.Value,
                "OrderItems"
            );

            if( orders == null || !orders.Any())
                throw new EntityNotFoundException($"No se encontraron órdenes para el cliente con ID {customerId.Value} y estado {status}");
        }
        else if (status != null)
        {
            if(!Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
                throw new ArgumentException($"El estado '{status}' no es válido.");
   
            orders = await _repository.GetFiltered<Order>(o => o.Status == parsedStatus,"OrderItems");
            if (orders == null || !orders.Any())
                throw new EntityNotFoundException($"No se encontraron órdenes con el estado {status}");
        }            
        else if (customerId.HasValue)
        {
            orders = await _repository.GetFiltered<Order>(o => o.CustomerID == customerId.Value, "OrderItems");
            if (orders == null || !orders.Any())
                throw new EntityNotFoundException($"No se encontraron órdenes para el cliente con ID {customerId.Value}");
        }

        if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
            orders = orders
                .OrderBy(o => o.Date) 
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);

        return orders.Select(order => new OrderModel.Response(
            order.Id,
            order.Date,
            order.CustomerID,
            order.ShippingAddress,
            order.BillingAddress,
            order.Status.ToString(),
            order.TotalAmount,
                (order.OrderItems ?? Enumerable.Empty<OrderItem>())
                .Select(oi => new OrderModel.OrderItemResponse(
                    oi.ProductId,
                    oi.Quantity,
                    oi.UnitPrice,
                    oi.Description)).ToList()
));

    }

    public async Task<OrderModel.ResponseId> UpdateOrderStatus(Guid id, string status)
    {
        _logger.LogInformation($"Actualizando el estado de la orden con ID {id} a {status}");
        if (id == Guid.Empty)
            throw new ArgumentException("El ID de la orden no puede ser un Guid vacío", nameof(id));

        var order = await _repository.GetById<Order>(id)
            ?? throw new EntityNotFoundException($"No se encontró la orden con ID {id}");

        if (!Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
            throw new ArgumentException($"El estado '{status}' no es válido.", nameof(status));

        if (order.Status == parsedStatus)
            return new OrderModel.ResponseId(order.Id);


        order.Status = parsedStatus; 
        
        await _repository.Update(order);
        _logger.LogInformation($"Estado de la orden {id} actualizado a {parsedStatus}");

        return new OrderModel.ResponseId(order.Id);
    }

   
}
