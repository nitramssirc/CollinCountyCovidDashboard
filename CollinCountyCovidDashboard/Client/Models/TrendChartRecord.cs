using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client.Models
{
    public class TrendChartRecord<T>
    {
        public DateTime Date { get; private set; }

        public T DailyValue { get; private set; }

        public T SevenDayAvg { get; private set; }

        public TrendChartRecord(DateTime date, T dailyValue, T sevenDayAvg)
        {
            Date = date;
            DailyValue = dailyValue;
            SevenDayAvg = sevenDayAvg;
        }

    }
}
