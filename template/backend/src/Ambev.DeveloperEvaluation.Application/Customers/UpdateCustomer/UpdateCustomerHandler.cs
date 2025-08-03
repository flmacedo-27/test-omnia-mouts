using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResult?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public UpdateCustomerHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<UpdateCustomerResult?> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (customer == null)
            return null;

        customer.Update(request.Name, request.Email, request.Phone, request.CustomerType, request.DocumentNumber, request.Active);
        
        var updatedCustomer = await _customerRepository.UpdateAsync(customer, cancellationToken);
        
        return _mapper.Map<UpdateCustomerResult>(updatedCustomer);
    }
} 