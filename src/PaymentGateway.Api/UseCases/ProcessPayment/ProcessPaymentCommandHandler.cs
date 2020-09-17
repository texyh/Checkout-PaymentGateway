using PaymentGateway.Domain.Crypto;
using PaymentGateway.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.UseCases.ProcessPayment
{
    public class ProcessPaymentCommandHandler : IProcessPaymentCommandHandler
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICryptoService _cryptoService;

        public ProcessPaymentCommandHandler(
            IPaymentRepository paymentRepository,
            ICryptoService cryptoService)
        {
            _paymentRepository = paymentRepository;
            _cryptoService = cryptoService;
        }

        public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand command)
        {

            //Validate Card;
            // Send Request to Acquiring bank;

            var encriptionKey = Guid.NewGuid().ToString("N");
            var encriptedCardNumber = _cryptoService.Encrypt(command.CardNumber, encriptionKey);
            var encriptedCardMonth = _cryptoService.Encrypt(command.CardExpiryMonth, encriptionKey);
            var encriptedCardDay = _cryptoService.Encrypt(command.CardExpiryYear, encriptionKey);
            var encriptedCardCVV = _cryptoService.Encrypt(command.CVV, encriptionKey);

            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                EncriptionKey = encriptionKey,
                CardNumber = encriptedCardNumber,
                CardExpiryMonth = encriptedCardMonth,
                CardExpiryYear = encriptedCardDay,
                CVV = encriptedCardCVV,
                BankPaymentIdentifier = Guid.NewGuid().ToString(),
                Amount = command.Amount,
                Currency = command.Currency
            };

            await _paymentRepository.Save(payment);

            return new SuccessResult(payment.Id);
        }
    }
}
