using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services
{
    public class CustomerManagementService
    {
        private readonly IRepository _repository;
       

        public CustomerManagementService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<CustomerModel.Response> AddCustomer(CustomerModel.Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("El nombre y el email no pueden estar vacíos");
            }
            var existingCustomer = await _repository.First<Customer>(c => c.Email == request.Email);
            if (existingCustomer != null)
            {
                throw new DuplicatedEntityException($"Ya existe un cliente con el email {request.Email}");
            }
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber.ToString()

            };
            await _repository.Add(customer);
            return new CustomerModel.Response(customer.Id, customer.Name, customer.Email, customer.PhoneNumber);
        }
        public async Task<CustomerModel.Response?> GetCustomerById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("El ID del cliente no puede ser un Guid vacío");
            }
            var customer = await _repository.GetById<Customer>(id);
            if (customer == null)
            {
                return null;
            }
            return new CustomerModel.Response(customer.Id, customer.Name, customer.Email, customer.PhoneNumber);
        }
        public async Task<List<CustomerModel.Response>> GetAllCustomers()
        {
            var customers = await _repository.GetAll<Customer>();
            return customers.Select(c => new CustomerModel.Response(c.Id, c.Name, c.Email, c.PhoneNumber)).ToList();
        }
    }
}
