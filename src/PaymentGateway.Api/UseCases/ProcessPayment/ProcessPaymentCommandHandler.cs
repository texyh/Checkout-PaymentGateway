using PaymentGateway.Api.Application.Crypto;
using PaymentGateway.Domain.AcquiringBank;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Domain.Payments.Commands;
using Serilog;
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

        private readonly IAquiringBankClient _acuquiryBank;

        private readonly ILogger _logger;

        public ProcessPaymentCommandHandler(
            IPaymentRepository paymentRepository,
            ICryptoService cryptoService,
            IAquiringBankClient acuquiryBank,
            ILogger logger)
        {
            _paymentRepository = paymentRepository;
            _cryptoService = cryptoService;
            _acuquiryBank = acuquiryBank;
            _logger = logger;
        }

        public async Task<ProcessPaymentResult> HandleAsync(ProcessPaymentCommand command)
        {
            _logger.Information("starting acquring bank payment request");
            var bankPayemntResult = await _acuquiryBank.ProcessPayment(new BankPaymentRequest 
            {
                Amount = command.Amount,
                Currency = command.Currency,
                CardExpiryYear = command.CardExpiryYear,
                CardExpiryMonth = command.CardExpiryMonth,
                CardNumber = command.CardNumber,
                CVV = command.CVV,
                MerchantId = Guid.NewGuid().ToString()
            });

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
                Amount = command.Amount,
                Currency = command.Currency,
                MerchantId = command.MerchantId,
            };

            payment.BankPaymentIdentifier = bankPayemntResult.PaymentIdentifier;
            payment.PaymentStatus = bankPayemntResult.PaymentStatus;

            await _paymentRepository.Save(payment);


            if(bankPayemntResult.PaymentStatus == PaymentStatus.Success) 
            {
                _logger.Information("bank payment successful");
                return new SuccessResult(payment.Id);
            } 
            else 
            {
                _logger.Error("The Bank was unable to process the payment");
                return new ErrorResult("The Bank was unable to process the payment");
            }

        }
    }
}
