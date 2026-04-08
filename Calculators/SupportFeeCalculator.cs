namespace LegacyRenewalApp.Calculators;

public class SupportFeeCalculator
{
    public decimal CalculateSupportFee(string planCode, bool includePremiumSupport)
    {
        if (!includePremiumSupport) return 0;
        return planCode switch
        {
            "START" => 250m,
            "PRO" => 400m,
            "ENTERPRISE" => 700m,
            _ => 0
        };
    }
}