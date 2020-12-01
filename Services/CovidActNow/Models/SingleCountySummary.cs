namespace Services.CovidActNow.Models
{

    public class SingleCountySummary
    {
        public string fips { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string level { get; set; }
        public object lat { get; set; }
        public string locationId { get; set; }
        public object _long { get; set; }
        public int population { get; set; }
        public Metrics metrics { get; set; }
        public Risklevels riskLevels { get; set; }
        public Actuals actuals { get; set; }
        public string lastUpdatedDate { get; set; }
    }

}
