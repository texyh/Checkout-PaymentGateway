using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.Abstractions
{
    public interface IReadOnlyRepository<T> 
    {
        Task<T> FindBy(Guid id);
    }
}
