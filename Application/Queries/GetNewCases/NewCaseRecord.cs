using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetNewCases
{
    public class NewCaseModel
    {
        public DateTime Date { get; private set; }

        public int NewCases { get; private set; }
        public decimal SevenDayAvg { get; private set; }

        public NewCaseModel(DateTime date, int newCases, decimal sevenDayAvg)
        {
            Date = date;
            NewCases = newCases;
            SevenDayAvg = sevenDayAvg;
        }

    }
}
