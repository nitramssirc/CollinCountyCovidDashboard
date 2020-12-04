using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StateOfTexas.Models
{
    public class DailyHospitalizationRecord
    {
        public DateTime Date { get; private set; }

        public int NewHopitalizations { get; private set; }

        public int TotalHospitalization { get; private set; }

        public DailyHospitalizationRecord(DateTime date, int newHopitalizations, int totalHospitalization)
        {
            NewHopitalizations = newHopitalizations;
            Date = date;
            TotalHospitalization = totalHospitalization;
        }
    }
}
