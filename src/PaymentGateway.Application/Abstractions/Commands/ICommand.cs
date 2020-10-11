using System;
using MediatR;

namespace PaymentGateway.Application.Abstractions.Commands
{
    public interface ICommand : IRequest
    {
        
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
        
    }
}