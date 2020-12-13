using System;
using System.Linq;
using System.Threading.Tasks;

using Application.BaseModels;
using Application.Data;

using Services.Common;
using Services.CovidActNow.Client;
using Services.CovidActNow.Models;
using Services.StateOfTexas.Client;
using Services.StateOfTexas.Models;

namespace Application.Queries.Get7DayAvg
{
    public class Get7DayAvgQuery : IGet7DayAvgQuery
    {
        #region Dependencies

        private readonly IStateOfTexasClient _stateOfTexasClient;

        #endregion

        #region Constructor

        public Get7DayAvgQuery(IStateOfTexasClient stateOfTexasClient)
        {
            _stateOfTexasClient = stateOfTexasClient;
        }

        #endregion

        #region Implementation of IGetTodayQuery

        public async Task<QueryResult<SevenDayAvg>> Execute()
        {
            var newCasesTask = _stateOfTexasClient.GetLatestNewCaseRecords(7);
            var testDataTask = _stateOfTexasClient.GetLatestPositiveTestCount(7);
            var hospitalDataTask = _stateOfTexasClient.GetLastestHospitalizationCount(7);
            var deathDataTask = _stateOfTexasClient.GetLatestDeathCount(7);
            await Task.WhenAll(newCasesTask, testDataTask, hospitalDataTask, deathDataTask);

            var newCasesResult = newCasesTask.Result;
            var testDataResult = testDataTask.Result;
            var hospitalDataResult = hospitalDataTask.Result;
            var deathDataResult = deathDataTask.Result;

            string error;
            if (!ProcessNewCases(newCasesResult,
                out var newCasesDate, out var newCasesCount, out var newCasesPer100k, out error) ||
                !ProcessTestData(testDataResult,
                out var testUpdateDate, out var testCount, out var positivityRate, out error) ||
                !ProcessHospitalData(hospitalDataResult,
                out var hospitalUpdateDate, out var totalHospitalizations, out var hospitalizationPct, out error) ||
                !ProcessDeathData(deathDataResult,
                out var deathUpdateDate, out var newDeaths, out var totalDeaths, out error))
            {
                return new QueryResult<SevenDayAvg>(error);
            }

            return new QueryResult<SevenDayAvg>(new SevenDayAvg(
                    newCasesCount,
                    newCasesPer100k,
                    newCasesDate,
                    testCount,
                    positivityRate,
                    testUpdateDate,
                    totalHospitalizations,
                    hospitalUpdateDate,
                    newDeaths,
                    totalDeaths,
                    deathUpdateDate,
                    hospitalizationPct
                ));
        }


        #endregion

        #region Private Methods

        private bool ProcessNewCases(ServiceResponse<NewCaseRecord[]> newCaseRecordResponse,
            out DateTime newCasesDate,
            out decimal newCasesCount,
            out decimal newCasesPer100k,
            out string error)
        {
            error = null;
            newCasesDate = DateTime.MinValue;
            newCasesCount = 0;
            newCasesPer100k = 0;

            if (!newCaseRecordResponse.WasSuccessful) { error = newCaseRecordResponse.Error; return false; }

            var newCaseRecords = newCaseRecordResponse.Response;
            newCasesDate = newCaseRecords.First().Date;
            newCasesCount = (decimal)newCaseRecords.Average(r=>r.NewCases);
            newCasesPer100k = newCasesCount / 10.34730M;

            return true;
        }

        private bool ProcessTestData(ServiceResponse<DailyTestData[]> response,
            out DateTime updateDate,
            out decimal testCount,
            out decimal positivityRate,
            out string error)
        {
            updateDate = DateTime.MinValue;
            testCount = 0;
            positivityRate = 0;
            error = null;

            if (!response.WasSuccessful) { error = response.Error; return false; }

            var testDataResults = response.Response;
            updateDate = testDataResults.First().Date;
            testCount = (decimal)testDataResults.Average(r=>r.Tests);
            positivityRate = testDataResults.Average(r=>r.PositivityRate);

            return true;
        }

        private bool ProcessHospitalData(ServiceResponse<DailyHospitalizationRecord[]> response,
            out DateTime hospitalUpdateDate,
            out int hospitalizedLastSevenDays,
            out decimal hospitalizationPct,
            out string error)
        {
            hospitalUpdateDate = DateTime.MinValue;
            hospitalizedLastSevenDays = 0;
            hospitalizationPct = 0;
            error = null;

            if (!response.WasSuccessful) { error = response.Error; return false; }

            var records = response.Response;
            hospitalUpdateDate = records.First().Date;
            hospitalizedLastSevenDays = records.Sum(r=>r.NewHopitalizations);
            hospitalizationPct = records.Average(r=>r.CovidPctOfCapacity);
            return true;
        }

        private bool ProcessDeathData(ServiceResponse<DailyDeathRecord[]> response, 
            out DateTime deathUpdateDate, 
            out decimal newDeaths, 
            out int totalDeaths, 
            out string error)
        {
            deathUpdateDate = DateTime.MinValue;
            newDeaths = 0;
            totalDeaths = 0;
            error = null;

            if (!response.WasSuccessful) { error = response.Error; return false; }

            var records = response.Response;
            deathUpdateDate = records.First().Date;
            newDeaths = (decimal)records.Average(r=>r.NewDeaths);
            totalDeaths = records.Sum(r=>r.NewDeaths);
            return true;
        }


        #endregion

    }
}
