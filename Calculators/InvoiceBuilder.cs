using System;

namespace LegacyRenewalApp.Calculators;

public class InvoiceBuilder
{
     private readonly DiscountCalculator _discountCalculator;
    private readonly SupportFeeCalculator _supportFeeCalculator;
    private readonly PaymentFeeCalculator _paymentFeeCalculator;
    private readonly TaxCalculator _taxCalculator;

    public InvoiceBuilder()
    {
        _discountCalculator = new DiscountCalculator();
        _supportFeeCalculator = new SupportFeeCalculator();
        _paymentFeeCalculator = new PaymentFeeCalculator();
        _taxCalculator = new TaxCalculator();
    }

    public RenewalInvoice BuildInvoice(
        Customer customer,
        SubscriptionPlan plan,
        int seatCount,
        string paymentMethod,
        bool includePremiumSupport,
        bool useLoyaltyPoints)
    {
        decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12) + plan.SetupFee;

        var (discount, discountNotes) = _discountCalculator.CalculateDiscounts(
            customer, plan, seatCount, baseAmount, useLoyaltyPoints);

        decimal afterDiscount = baseAmount - discount;
        if (afterDiscount < 300m)
        {
            afterDiscount = 300m;
            discountNotes += "minimum discounted subtotal applied; ";
        }

        decimal supportFee = _supportFeeCalculator.CalculateSupportFee(plan.Code, includePremiumSupport);
        if (supportFee > 0) discountNotes += "premium support included; ";

        decimal paymentFeeBase = afterDiscount + supportFee;
        decimal paymentFee = _paymentFeeCalculator.CalculatePaymentFee(paymentMethod, paymentFeeBase);
        discountNotes += GetPaymentNote(paymentMethod);

        decimal taxBase = afterDiscount + supportFee + paymentFee;
        decimal tax = _taxCalculator.CalculateTax(customer.Country, taxBase);

        decimal finalAmount = taxBase + tax;
        if (finalAmount < 500m)
        {
            finalAmount = 500m;
            discountNotes += "minimum invoice amount applied; ";
        }

        return new RenewalInvoice
        {
            InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customer.Id}-{plan.Code}",
            CustomerName = customer.FullName,
            PlanCode = plan.Code,
            PaymentMethod = paymentMethod,
            SeatCount = seatCount,
            BaseAmount = MoneyRounder.Round(baseAmount),
            DiscountAmount = MoneyRounder.Round(discount),
            SupportFee = MoneyRounder.Round(supportFee),
            PaymentFee = MoneyRounder.Round(paymentFee),
            TaxAmount = MoneyRounder.Round(tax),
            FinalAmount = MoneyRounder.Round(finalAmount),
            Notes = discountNotes.Trim(),
            GeneratedAt = DateTime.UtcNow
        };
    }

    private string GetPaymentNote(string paymentMethod) =>
        paymentMethod switch
        {
            "CARD" => "card payment fee; ",
            "BANK_TRANSFER" => "bank transfer fee; ",
            "PAYPAL" => "paypal fee; ",
            "INVOICE" => "invoice payment; ",
            _ => ""
        };
}