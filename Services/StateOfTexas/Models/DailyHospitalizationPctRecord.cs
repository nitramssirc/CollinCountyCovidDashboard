using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StateOfTexas.Models
{
    public class DailyCovidHospitalizationPctRecord
    {
        public DateTime Date { get; private set; }

        public decimal Pct { get; private set; }

        public DailyCovidHospitalizationPctRecord(decimal pct, DateTime date)
        {
            Pct = pct;
            Date = date;
        }
    }
}
