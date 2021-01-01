using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StateOfTexas.Models
{
    public class DailyHospitalBedRecord
    {
        public DateTime Date { get; private set; }
        public int Available { get; private set; }
        public int TotalOccupied { get; private set; }
        public int CovidOccupied { get; private set; }
        public int Capacity { get; private set; }

        public DailyHospitalBedRecord(DateTime date, int available, int totalOccupied, int covidOccupied)
        {
            Date = date;
            Available = available;
            TotalOccupied = totalOccupied;
            CovidOccupied = covidOccupied;
            Capacity = Available + TotalOccupied;
        }
    }
}
