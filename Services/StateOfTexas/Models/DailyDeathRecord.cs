using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StateOfTexas.Models
{
    public class DailyDeathRecord
    {
        public DateTime Date { get; private set; }

        public int NewDeaths { get; private set; }

        public int TotalDeaths { get; private set; }

        public DailyDeathRecord(DateTime date, int newDeaths, int totalDeaths)
        {
            Date = date;
            NewDeaths = newDeaths;
            TotalDeaths = totalDeaths;
        }
    }
}
