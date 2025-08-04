using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public class DeleteProductCommand : IRequest<DeleteProductResult>
{
    public Guid Id { get; set; }
} 