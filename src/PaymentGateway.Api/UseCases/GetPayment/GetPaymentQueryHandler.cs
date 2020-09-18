using System.Threading.Tasks;
using PaymentGateway.Domain.Crypto;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Domain.Payments.Queries;
using PaymentGateway.Domain.Helpers;
namespace PaymentGateway.Api.UseCases.GetPayment
{
    public class GetPaymentQueryHandler : IGetPaymentQueryHandler
    {
        private readonly IPaymentRepository _paymentRepository;
        
        private readonly ICryptoService _cryptoService;

        public GetPaymentQueryHandler(IPaymentRepository paymentRepository, ICryptoService cryptoService)
        {
            _paymentRepository = paymentRepository;
            _cryptoService = cryptoService;
        }

        public async Task<GetPaymentResult> HandleAsync(GetPaymentQuery query)
        {
            var payment = await _paymentRepository.FindBy(query.PaymentId);

            if(payment == null) 
            {
                return new ErrorResult($"There is no payment with id: {query.PaymentId}");
            }

            var cardNumber = _cryptoService.Decrypt(payment.CardNumber, payment.EncriptionKey);

            return new SuccessResult 
            {
                CardNumber = cardNumber.Mask(),
                Amount = payment.Amount,
                PaymentDate = payment.CreatedDate,
                PaymentStatus = payment.PaymentStatus,
                Currency = payment.Currency
            };
        }
    }
}