namespace Services.CovidActNow.Models
{
    public class Actuals
    {
        public int cases { get; set; }
        public int deaths { get; set; }
        public int positiveTests { get; set; }
        public int negativeTests { get; set; }
        public object contactTracers { get; set; }
        public Hospitalbeds hospitalBeds { get; set; }
        public Icubeds icuBeds { get; set; }
        public int newCases { get; set; }
    }

}
