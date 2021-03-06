﻿using System;
using System.Linq;
using System.Threading.Tasks;

using Application.BaseModels;
using Application.Data;

using Services.Common;
using Services.CovidActNow.Client;
using Services.CovidActNow.Models;
using Services.StateOfTexas.Client;
using Services.StateOfTexas.Models;

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
            var newCasesTask = _stateOfTexasClient.GetLatestNewCaseRecords(1);
            var testDataTask = _stateOfTexasClient.GetLatestPositiveTestCount(1);
            var hospitalDataTask = _stateOfTexasClient.GetLastestHospitalizationCount(1);
            var deathDataTask = _stateOfTexasClient.GetLatestDeathCount(1);
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
                return new QueryResult<Today>(error);
            }

            return new QueryResult<Today>(new Today(
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
            out int newCasesCount,
            out decimal newCasesPer100k,
            out string error)
        {
            error = null;
            newCasesDate = DateTime.MinValue;
            newCasesCount = 0;
            newCasesPer100k = 0;

            if (!newCaseRecordResponse.WasSuccessful) { error = newCaseRecordResponse.Error; return false; }

            var newCaseRecord = newCaseRecordResponse.Response.First();
            newCasesDate = newCaseRecord.Date;
            newCasesCount = newCaseRecord.NewCases;
            newCasesPer100k = newCasesCount / 10.34730M;

            return true;
        }

        private bool ProcessTestData(ServiceResponse<DailyTestData[]> response,
            out DateTime updateDate,
            out int testCount,
            out decimal positivityRate,
            out string error)
        {
            updateDate = DateTime.MinValue;
            testCount = 0;
            positivityRate = 0;
            error = null;

            if (!response.WasSuccessful) { error = response.Error; return false; }

            var testDataResult = response.Response.First();
            updateDate = testDataResult.Date;
            testCount = testDataResult.Tests;
            positivityRate = testDataResult.PositivityRate;

            return true;
        }

        private bool ProcessHospitalData(ServiceResponse<DailyHospitalizationRecord[]> response,
            out DateTime hospitalUpdateDate,
            out int totalHospitalizations,
            out decimal hospitalizationPct,
            out string error)
        {
            hospitalUpdateDate = DateTime.MinValue;
            totalHospitalizations = 0;
            hospitalizationPct = 0;
            error = null;

            if (!response.WasSuccessful) { error = response.Error; return false; }

            var record = response.Response.First();
            hospitalUpdateDate = record.Date;
            totalHospitalizations = record.TotalHospitalization;
            hospitalizationPct = record.CovidPctOfCapacity;
            return true;
        }

        private bool ProcessDeathData(ServiceResponse<DailyDeathRecord[]> response, 
            out DateTime deathUpdateDate, 
            out int newDeaths, 
            out int totalDeaths, 
            out string error)
        {
            deathUpdateDate = DateTime.MinValue;
            newDeaths = 0;
            totalDeaths = 0;
            error = null;

            if (!response.WasSuccessful) { error = response.Error; return false; }

            var record = response.Response.First();
            deathUpdateDate = record.Date;
            newDeaths = record.NewDeaths;
            totalDeaths = record.TotalDeaths;
            return true;
        }


        #endregion

    }
}
