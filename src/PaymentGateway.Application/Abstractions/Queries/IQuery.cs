using MediatR;

namespace PaymentGateway.Application.Abstractions.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {

    }
}