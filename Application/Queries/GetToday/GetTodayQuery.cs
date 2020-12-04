using System;
using System.Threading.Tasks;

using Application.BaseModels;
using Application.Data;

using Services.Common;
using Services.CovidActNow.Client;
using Services.CovidActNow.Models;
using Services.StateOfTexas.Client;

namespace Application.Queries.GetToday
{
    public class GetTodayQuery : IGetTodayQuery
    {
        #region Dependencies

        private readonly IStateOfTexasClient _stateOfTexasClient;

        #endregion

        #region Constructor

        public GetTodayQuery(IStateOfTexasClient stateOfTexasClient)
        {
            _stateOfTexasClient = stateOfTexasClient;
        }

        #endregion

        #region Implementation of IGetTodayQuery

        public async Task<QueryResult<Today>> Execute()
        {
            var newCasesTask = _stateOfTexasClient.GetLatestNewCaseCount();
            var testDataTask = _stateOfTexasClient.GetLatestPositiveTestCount();
            var hospitalDataTask = _stateOfTexasClient.GetLastestHospitalizationCount();
            var deathDataTask = _stateOfTexasClient.GetLatestDeathCount();
            await Task.WhenAll(newCasesTask, testDataTask, hospitalDataTask, deathDataTask);

            var newCasesResult = newCasesTask.Result;
            var testDataResult = testDataTask.Result;
            var hospitalDataResult = hospitalDataTask.Result;
            var deathDataResult = deathDataTask.Result;
            if (!newCasesResult.WasSuccessful) return new QueryResult<Today>(newCasesResult.Error);
            if (!testDataResult.WasSuccessful) return new QueryResult<Today>(testDataResult.Error);
            if (!hospitalDataResult.WasSuccessful) return new QueryResult<Today>(hospitalDataResult.Error);
            if (!deathDataResult.WasSuccessful) return new QueryResult<Today>(deathDataResult.Error);

            var newCasesDate = newCasesResult.Response.Date;
            var newCasesCount = newCasesResult.Response.NewCases;
            var newCasesPer100k = newCasesCount / 10.34730M;

            var testUpdateDate = testDataResult.Response.Date;
            var testCount = testDataResult.Response.Tests;
            var positivityRate = testDataResult.Response.PositivityRate;

            var hospitalUpdateDate = hospitalDataResult.Response.Date;
            var newHospitalizations = hospitalDataResult.Response.NewHopitalizations;
            var totalHospitalizations = hospitalDataResult.Response.TotalHospitalization;

            var deathUpdateDate = deathDataResult.Response.Date;
            var newDeaths = deathDataResult.Response.NewDeaths;
            var totalDeaths = deathDataResult.Response.TotalDeaths;

            return new QueryResult<Today>(new Today(
                    newCasesCount,
                    newCasesPer100k,
                    newCasesDate,
                    testCount,
                    positivityRate,
                    testUpdateDate,
                    newHospitalizations,
                    totalHospitalizations,
                    hospitalUpdateDate,
                    newDeaths,
                    totalDeaths,
                    deathUpdateDate
                ));
        }


        #endregion

    }
}
