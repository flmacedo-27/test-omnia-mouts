using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductCommand : IRequest<GetProductResult?>
{
    public Guid Id { get; set; }
} 