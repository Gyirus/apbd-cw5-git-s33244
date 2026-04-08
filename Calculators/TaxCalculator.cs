namespace LegacyRenewalApp.Calculators;

public class TaxCalculator
{
    public decimal CalculateTax(string country, decimal taxableAmount)
    {
        decimal rate = country switch
        {
            "Poland" => 0.23m,
            "Germany" => 0.19m,
            "Czech Republic" => 0.21m,
            "Norway" => 0.25m,
            _ => 0.20m
        };
        return taxableAmount * rate;
    }
}