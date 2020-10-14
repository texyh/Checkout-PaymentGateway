using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.Abstractions
{
    public interface IAggregateStore<T> where T : AggregateRoot
    {
        Task AppendChanges(T aggreatet);

        Task<T> Load(Guid Id);
    }
}
