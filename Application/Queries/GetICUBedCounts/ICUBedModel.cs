using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetICUBedCounts
{
    public class ICUBedModel
    {
        public DateTime Date { get; private set; }
        public int Total { get; private set; }

        public int Occupied { get; private set; }

        public int CovidOccupied { get; private set; }

        public ICUBedModel(DateTime date, int total, int occupied, int covidOccupied)
        {
            Date = date;
            Total = total;
            Occupied = occupied;
            CovidOccupied = covidOccupied;
        }
    }
}
