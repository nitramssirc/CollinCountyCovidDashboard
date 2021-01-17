using System.ComponentModel;

public enum NavPageType
{
    Latest,
    SevenDayAvg,
    VaccineInfo,
    NewCaseTrends,
    NewDeathsTrend,
    PositivityRateTrend,
    HospitalBedsTrend,
    ICUBedsTrend,
    Sources
}

public static class NavPageTypeExt
{
    public static string Description(this NavPageType navPageType)
    {
        return navPageType switch
        {
            NavPageType.SevenDayAvg => "7 Day Averages",
            NavPageType.NewCaseTrends => "New Cases Trend",
            NavPageType.VaccineInfo => "Vaccine Info",
            NavPageType.NewDeathsTrend => "New Deaths Trend",
            NavPageType.PositivityRateTrend => "Positivity Rate Trend",
            NavPageType.HospitalBedsTrend => "Hospital Beds Trend",
            NavPageType.ICUBedsTrend => "ICU Beds Trend",
            _ => navPageType.ToString(),
        };
    }
}
