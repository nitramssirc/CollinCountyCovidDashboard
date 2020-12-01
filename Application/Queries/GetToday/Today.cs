using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetToday
{
    public class Today
    {
        public int Cases { get; private set; }

        public decimal CasesPer100K { get; private set; }

        public int Tests { get; private set; }

        public decimal PositivityRate { get; private set; }

        public int Hospitalizations { get; private set; }

        public int TotalCurrentlyHospitalized { get; private set; }

        public int Deaths { get; private set; }

        public int TotalDeaths { get; private set; }

        public DateTime LastUpdated { get; private set; }

        public Today(int cases,
                     decimal casesPer100K,
                     int tests,
                     decimal positivityRate,
                     int hospitalizations,
                     int totalCurrentlyHospitalized,
                     int deaths,
                     int totalDeaths,
                     DateTime lastUpdated)
        {
            Cases = cases;
            CasesPer100K = casesPer100K;
            Tests = tests;
            PositivityRate = positivityRate;
            Hospitalizations = hospitalizations;
            TotalCurrentlyHospitalized = totalCurrentlyHospitalized;
            Deaths = deaths;
            TotalDeaths = totalDeaths;
            LastUpdated = lastUpdated;
        }

    }
}
