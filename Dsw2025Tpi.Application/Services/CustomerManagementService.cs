using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dsw2025Tpi.Application.Services
{
    public class CustomerManagementService
    {
        private readonly IRepository _repository;
        private readonly ILogger<CustomerManagementService> _logger;   


        public CustomerManagementService(IRepository repository, ILogger<CustomerManagementService> logger)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task<CustomerModel.Response> AddCustomer(CustomerModel.Request request)
        {
            _logger.LogInformation($"Agregando cliente: {request.Name} con email: {request.Email}");
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("El nombre y el email no pueden estar vacíos");
            
            var existingCustomer = await _repository.First<Customer>(c => c.Email == request.Email);
            if (existingCustomer != null)
                throw new DuplicatedEntityException($"Ya existe un cliente con el email {request.Email}");
            
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber.ToString()

            };
            await _repository.Add(customer);
            return new CustomerModel.Response(
                customer.Id,
                customer.Name,
                customer.Email,
                customer.PhoneNumber
            );
        }
        public async Task<CustomerModel.Response?> GetCustomerById(Guid id)
        {
            _logger.LogInformation($"Obteniendo cliente con ID: {id}");
            if (id == Guid.Empty)
                throw new ArgumentException("El ID del cliente no puede ser un Guid vacío");
            
            var customer = await _repository.GetById<Customer>(id) ??
                throw new EntityNotFoundException($"No existe un cliente con el Id {id}");

            return new CustomerModel.Response(
                customer.Id,
                customer.Name,
                customer.Email,
                customer.PhoneNumber
            );
        }
        public async Task<List<CustomerModel.Response>> GetAllCustomers()
        {
            _logger.LogInformation("Obteniendo todos los clientes");
            var customers = await _repository.GetAll<Customer>();
            if (customers == null || !customers.Any())
                throw new EntityNotFoundException("No existen clientes registrados");
            
            return customers.Select(c => new CustomerModel.Response(
                c.Id,
                c.Name,
                c.Email,
                c.PhoneNumber
            )).ToList();
        }
    }
}
