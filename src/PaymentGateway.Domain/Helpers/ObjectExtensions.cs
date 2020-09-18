using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Helpers
{
    public static class ObjectExtensions
    {

        public static string Mask(this string cardNumber)
        {
            if(cardNumber.Length < 5)
            {
                return cardNumber;
            }

            var lastFourDigits = cardNumber.Substring(cardNumber.Length - 4, 4);
            return lastFourDigits.PadLeft(cardNumber.Length, '*');
        }
    }
}
