using System.ComponentModel;

public enum NavPageType
{
    Latest,
    SevenDayAvg,
    NewCaseTrends,
    NewDeathsTrend,
    PositivityRateTrend,
    HospitalBedsTrend,
    ICUBedsTrend,
    Sources
}

public static class NavePageTypeExt
{
    public static string Description(this NavPageType navPageType)
    {
        return navPageType switch
        {
            NavPageType.SevenDayAvg => "7 Day Averages",
            NavPageType.NewCaseTrends => "New Cases Trend",
            NavPageType.NewDeathsTrend => "New Deaths Trend",
            NavPageType.PositivityRateTrend => "Positivity Rate Trend",
            NavPageType.HospitalBedsTrend => "Hospital Beds Trend",
            NavPageType.ICUBedsTrend => "ICU Beds Trend",
            _ => navPageType.ToString(),
        };
    }
}
