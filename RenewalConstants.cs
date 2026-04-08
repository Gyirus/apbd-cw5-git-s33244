namespace LegacyRenewalApp;

public class RenewalConstants
{
    public const decimal SilverSegmentDiscount = 0.05m;
    public const decimal GoldSegmentDiscount = 0.10m;
    public const decimal PlatinumSegmentDiscount = 0.15m;
    public const decimal EducationSegmentDiscount = 0.20m;
    
    public const decimal LongTermLoyaltyDiscount = 0.07m;
    public const decimal BasicLoyaltyDiscount = 0.03m;
    public const int LongTermLoyaltyYears = 5;
    public const int BasicLoyaltyYears = 2;
    
    public const decimal LargeTeamDiscount = 0.12m;
    public const decimal MediumTeamDiscount = 0.08m;
    public const decimal SmallTeamDiscount = 0.04m;
    public const int LargeTeamThreshold = 50;
    public const int MediumTeamThreshold = 20;
    public const int SmallTeamThreshold = 10;
    
    public const int MaxLoyaltyPointsToUse = 200;
    
    public const decimal MinimumDiscountedSubtotal = 300m;
    public const decimal MinimumFinalAmount = 500m;
    
    public const decimal StartSupportFee = 250m;
    public const decimal ProSupportFee = 400m;
    public const decimal EnterpriseSupportFee = 700m;
    
    public const decimal CardPaymentFee = 0.02m;
    public const decimal BankTransferPaymentFee = 0.01m;
    public const decimal PayPalPaymentFee = 0.035m;
    
    public const decimal PolandTaxRate = 0.23m;
    public const decimal GermanyTaxRate = 0.19m;
    public const decimal CzechRepublicTaxRate = 0.21m;
    public const decimal NorwayTaxRate = 0.25m;
    public const decimal DefaultTaxRate = 0.20m;
    
    public const int MonthsInYear = 12;
    public const int RoundingDecimals = 2;
}