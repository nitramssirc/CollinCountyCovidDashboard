using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Get7DayAvg
{
    public class SevenDayAvg
    {
        public decimal NewCaseAvg { get; private set; }

        public decimal NewCasesAvgPer100K { get; private set; }

        public DateTime CasesUpdateDate { get; private set; }

        public decimal TestsPerDayAvg { get; private set; }

        public decimal PositivityRate { get; private set; }

        public DateTime TestsUpdateDate { get; private set; }

        public decimal CovidPercentOfHospitalizationsAvg { get; private set; }

        public decimal NewHospitalizationsSevenDayTotal { get; private set; }

        public DateTime HospitalizationsUpdateDate { get; private set; }

        public decimal NewDeathsAvg { get; private set; }

        public int NewDeathsSevenDayTotal { get; private set; }

        public DateTime DeathsUpdateDate { get; private set; }

        public SevenDayAvg(decimal newCaseAvg,
                     decimal newCasesAvgPer100k,
                     DateTime casesUpdateDate,
                     decimal testsPerDayAvg,
                     decimal positivityRate,
                     DateTime testsUpdateDate,
                     decimal newHopitalizationsSevenDayTotal,
                     DateTime hospitalizationsUpdateDate,
                     decimal newDeathsAvg,
                     int newDeathsSevenDayTotal,
                     DateTime deathsUpdateDate, 
                     decimal covidPercentOfHospitalizationsAvg)
        {
            NewCaseAvg = newCaseAvg;
            NewCasesAvgPer100K = newCasesAvgPer100k;
            CasesUpdateDate = casesUpdateDate;
            TestsPerDayAvg = testsPerDayAvg;
            PositivityRate = positivityRate;
            TestsUpdateDate = testsUpdateDate;
            NewHospitalizationsSevenDayTotal = newHopitalizationsSevenDayTotal;
            HospitalizationsUpdateDate = hospitalizationsUpdateDate;
            NewDeathsAvg = newDeathsAvg;
            NewDeathsSevenDayTotal = newDeathsSevenDayTotal;
            DeathsUpdateDate = deathsUpdateDate;
            CovidPercentOfHospitalizationsAvg = covidPercentOfHospitalizationsAvg;
        }

    }
}
