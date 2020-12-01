using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StateOfTexas.Models
{
    public class NewCaseRecord
    {
        public int NewCases { get; private set; }

        public DateTime Date { get; private set; }

        public NewCaseRecord(int newCases,
                             DateTime date)
        {
            NewCases = newCases;
            Date = date;
        }
    }
}
