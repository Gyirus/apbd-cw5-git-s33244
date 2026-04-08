using System;

namespace LegacyRenewalApp.Calculators;

public class MoneyRounder
{
    public static decimal Round(decimal value) =>
        Math.Round(value, 2, MidpointRounding.AwayFromZero);
}