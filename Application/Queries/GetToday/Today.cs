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

        public DateTime CasesUpdateDate { get; private set; }

        public int Tests { get; private set; }

        public decimal PositivityRate { get; private set; }

        public DateTime TestsUpdateDate { get; private set; }

        public decimal CovidPercentOfHospitalizations { get; private set; }

        public int TotalCurrentlyHospitalized { get; private set; }

        public DateTime HospitalizationsUpdateDate { get; private set; }

        public int Deaths { get; private set; }

        public int TotalDeaths { get; private set; }

        public DateTime DeathsUpdateDate { get; private set; }

        public Today(int cases,
                     decimal casesPer100K,
                     DateTime casesUpdateDate,
                     int tests,
                     decimal positivityRate,
                     DateTime testsUpdateDate,
                     int totalCurrentlyHospitalized,
                     DateTime hospitalizationsUpdateDate,
                     int deaths,
                     int totalDeaths,
                     DateTime deathsUpdateDate, 
                     decimal covidPercentOfHospitalizations)
        {
            Cases = cases;
            CasesPer100K = casesPer100K;
            CasesUpdateDate = casesUpdateDate;
            Tests = tests;
            PositivityRate = positivityRate;
            TestsUpdateDate = testsUpdateDate;
            TotalCurrentlyHospitalized = totalCurrentlyHospitalized;
            HospitalizationsUpdateDate = hospitalizationsUpdateDate;
            Deaths = deaths;
            TotalDeaths = totalDeaths;
            DeathsUpdateDate = deathsUpdateDate;
            CovidPercentOfHospitalizations = covidPercentOfHospitalizations;
        }

    }
}
