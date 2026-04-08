namespace LegacyRenewalApp.Calculators;

public class DiscountCalculator
{
    public (decimal TotalDiscount, string Notes) CalculateDiscounts(
        Customer customer,
        SubscriptionPlan plan,
        int seatCount,
        decimal baseAmount,
        bool useLoyaltyPoints)
    {
        decimal discount = 0;
        string notes = "";

        decimal segmentDiscount = GetSegmentDiscount(customer, plan, baseAmount);
        discount += segmentDiscount;
        notes += GetSegmentNote(customer, plan);

        decimal loyaltyDiscount = GetLoyaltyDiscount(customer, baseAmount);
        discount += loyaltyDiscount;
        notes += GetLoyaltyNote(customer);

        decimal volumeDiscount = GetVolumeDiscount(seatCount, baseAmount);
        discount += volumeDiscount;
        notes += GetVolumeNote(seatCount);

        decimal pointsDiscount = GetLoyaltyPointsDiscount(customer, useLoyaltyPoints);
        discount += pointsDiscount;
        notes += GetPointsNote(customer, useLoyaltyPoints);

        return (discount, notes.Trim());
    }

    private decimal GetSegmentDiscount(Customer customer, SubscriptionPlan plan, decimal baseAmount)
    {
        switch (customer.Segment)
        {
            case "Silver": return baseAmount * 0.05m;
            case "Gold":   return baseAmount * 0.10m;
            case "Platinum": return baseAmount * 0.15m;
            case "Education": 
                return plan.IsEducationEligible ? baseAmount * 0.20m : 0;
            default: return 0;
        }
    }

    private string GetSegmentNote(Customer customer, SubscriptionPlan plan)
    {
        switch (customer.Segment)
        {
            case "Silver": return "silver discount; ";
            case "Gold": return "gold discount; ";
            case "Platinum": return "platinum discount; ";
            case "Education": 
                return plan.IsEducationEligible ? "education discount; " : "";
            default: return "";
        }
    }

    private decimal GetLoyaltyDiscount(Customer customer, decimal baseAmount)
    {
        if (customer.YearsWithCompany >= 5)
            return baseAmount * 0.07m;
        else if (customer.YearsWithCompany >= 2)
            return baseAmount * 0.03m;
        else
            return 0;
    }

    private string GetLoyaltyNote(Customer customer)
    {
        if (customer.YearsWithCompany >= 5)
            return "long-term loyalty discount; ";
        else if (customer.YearsWithCompany >= 2)
            return "basic loyalty discount; ";
        else
            return "";
    }

    private decimal GetVolumeDiscount(int seatCount, decimal baseAmount)
    {
        if (seatCount >= 50)
            return baseAmount * 0.12m;
        else if (seatCount >= 20)
            return baseAmount * 0.08m;
        else if (seatCount >= 10)
            return baseAmount * 0.04m;
        else
            return 0;
    }

    private string GetVolumeNote(int seatCount)
    {
        if (seatCount >= 50)
            return "large team discount; ";
        else if (seatCount >= 20)
            return "medium team discount; ";
        else if (seatCount >= 10)
            return "small team discount; ";
        else
            return "";
    }

    private decimal GetLoyaltyPointsDiscount(Customer customer, bool useLoyaltyPoints)
    {
        if (!useLoyaltyPoints || customer.LoyaltyPoints <= 0)
            return 0;
        
        int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;
        return pointsToUse;
    }

    private string GetPointsNote(Customer customer, bool useLoyaltyPoints)
    {
        if (!useLoyaltyPoints || customer.LoyaltyPoints <= 0)
            return "";
        
        int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;
        return $"loyalty points used: {pointsToUse}; ";
    }

}