namespace Services.CovidActNow.Models
{
    public class Icubeds
    {
        public int capacity { get; set; }
        public object currentUsageTotal { get; set; }
        public int currentUsageCovid { get; set; }
        public float typicalUsageRate { get; set; }
    }

}
