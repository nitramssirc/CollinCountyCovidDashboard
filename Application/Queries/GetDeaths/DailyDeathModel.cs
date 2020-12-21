using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetDeaths
{
    public class DailyDeathModel
    {
        public DateTime Date { get; private set; }

        public int Deaths { get; private set; }
        public decimal SevenDayAvg { get; private set; }

        public DailyDeathModel(DateTime date, int deaths, decimal sevenDayAvg)
        {
            Date = date;
            Deaths = deaths;
            SevenDayAvg = sevenDayAvg;
        }

    }
}
