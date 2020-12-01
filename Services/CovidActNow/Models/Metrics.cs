namespace Services.CovidActNow.Models
{
    public class Metrics
    {
        public float testPositivityRatio { get; set; }
        public Testpositivityratiodetails testPositivityRatioDetails { get; set; }
        public float caseDensity { get; set; }
        public object contactTracerCapacityRatio { get; set; }
        public float infectionRate { get; set; }
        public float infectionRateCI90 { get; set; }
        public float icuHeadroomRatio { get; set; }
        public Icuheadroomdetails icuHeadroomDetails { get; set; }
    }

}
