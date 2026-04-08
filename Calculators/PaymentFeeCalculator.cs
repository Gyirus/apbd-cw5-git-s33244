using System;

namespace LegacyRenewalApp.Calculators;

public class PaymentFeeCalculator
{
    public decimal CalculatePaymentFee(string paymentMethod, decimal amountBeforeFee)
    {
        return paymentMethod switch
        {
            "CARD" => amountBeforeFee * 0.02m,
            "BANK_TRANSFER" => amountBeforeFee * 0.01m,
            "PAYPAL" => amountBeforeFee * 0.035m,
            "INVOICE" => 0,
            _ => throw new ArgumentException("Unsupported payment method")
        };
    }
}