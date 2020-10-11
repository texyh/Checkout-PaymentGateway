using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Application.Abstractions.Queries;
using PaymentGateway.Application.Crypto;
using PaymentGateway.Application.Payments.GetPayment;
using PaymentGateway.Domain.Helpers;
using PaymentGateway.Domain.Payments;
using Serilog;

namespace PaymentGateway.Api.UseCases.GetPayment
{
    public class GetPaymentQueryHandler : IQueryHandler<GetPaymentQuery, GetPaymentResult>
    {
        private readonly IPaymentRepository _paymentRepository;
        
        private readonly ICryptoService _cryptoService;

        private readonly ILogger _logger;

        public GetPaymentQueryHandler(
            IPaymentRepository paymentRepository,
            ICryptoService cryptoService,
            ILogger logger)
        {
            _paymentRepository = paymentRepository;
            _cryptoService = cryptoService;
            _logger = logger;
        }

        public async Task<GetPaymentResult> Handle(GetPaymentQuery query, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.FindBy(query.PaymentId);

            if(payment == null) 
            {
                _logger.Error($"There is no payment with id: {query.PaymentId}");
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