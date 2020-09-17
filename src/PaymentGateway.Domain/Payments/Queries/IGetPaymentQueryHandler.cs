using System.Threading.Tasks;

namespace PaymentGateway.Domain.Payments.Queries
{
    public interface IGetPaymentQueryHandler 
    {
        Task<GetPaymentResult> HandleAsync(GetPaymentQuery query);
    }
}