namespace Services.CovidActNow.Models
{
    public class Risklevels
    {
        public int overall { get; set; }
        public int testPositivityRatio { get; set; }
        public int caseDensity { get; set; }
        public int contactTracerCapacityRatio { get; set; }
        public int infectionRate { get; set; }
        public int icuHeadroomRatio { get; set; }
    }

}
