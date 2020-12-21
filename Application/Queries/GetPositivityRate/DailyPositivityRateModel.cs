using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetPositivityRate
{
    public class DailyPositivityRateModel
    {
        public DateTime Date { get; private set; }

        public decimal PositivityRate { get; private set; }
        public decimal SevenDayAvg { get; private set; }

        public DailyPositivityRateModel(DateTime date, decimal positivityRate, decimal sevenDayAvg)
        {
            Date = date;
            PositivityRate = positivityRate;
            SevenDayAvg = sevenDayAvg;
        }

    }
}
