using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StateOfTexas.Models
{
    public class DailyTestData
    {
        public DateTime Date { get; private set; }

        public int Tests { get; private set; }

        public DailyTestData(DateTime date,
                             int tests)
        {
            Date = date;
            Tests = tests;
        }
    }
}
