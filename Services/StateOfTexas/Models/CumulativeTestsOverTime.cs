using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StateOfTexas.Models
{
    public class CumulativeTestsOverTime
    {
        public DailyTestData[] DailyTests { get; private set; }

        public CumulativeTestsOverTime(DailyTestData[] dailyTests)
        {
            DailyTests = dailyTests;
        }
    }
}
