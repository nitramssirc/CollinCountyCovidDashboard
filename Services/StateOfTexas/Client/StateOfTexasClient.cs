﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using ExcelDataReader;

using Services.Common;
using Services.StateOfTexas.Models;

namespace Services.StateOfTexas.Client
{
    public class StateOfTexasClient : IStateOfTexasClient
    {

        #region Implementation of GetCumulativeTestsOverTimeForCounty

        public async Task<ServiceResponse<NewCaseRecord[]>> GetLatestNewCaseRecords(int numDays)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var newCaseDataSet = LoadExcelDataAsDataSet("TexasCOVID19NewConfirmedCasesbyCounty.xlsx");
                    var newCases = GetNewCaseDataFromDataSet(newCaseDataSet.Tables[0]).OrderByDescending(r=>r.Date);

                    var returnRecords = newCases.Take(numDays).ToArray();
                    return new ServiceResponse<NewCaseRecord[]>(returnRecords);
                }
                catch (Exception ex)
                {
                    return new ServiceResponse<NewCaseRecord[]>(
                        "An error occurred loading the new case data from the State of Texas", ex);
                }
            }
            );
        }

        public async Task<ServiceResponse<int>> GetTotalCases()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var newCaseDataSet = LoadExcelDataAsDataSet("TexasCOVID19NewConfirmedCasesbyCounty.xlsx");
                    var newCases = GetNewCaseDataFromDataSet(newCaseDataSet.Tables[0]);
                    var totalCases = newCases.Sum(nc => nc.NewCases);

                    return new ServiceResponse<int>(totalCases);
                }
                catch (Exception ex)
                {
                    return new ServiceResponse<int>(
                        "An error occurred loading the total cases data from the State of Texas", ex);
                }
            }
            );
        }

        public async Task<ServiceResponse<DailyTestData[]>> GetLatestPositiveTestCount(int numDays)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var testDataSet = LoadExcelDataAsDataSet("CumulativeTestsOverTimeByCounty.xlsx");
                    var testData = GetDailyTestCaseCount(testDataSet.Tables[0]).OrderByDescending(t=>t.Item1);
                    var latestTestCases = testData.Take(numDays);

                    var newCaseDataSet = LoadExcelDataAsDataSet("TexasCOVID19NewConfirmedCasesbyCounty.xlsx");
                    var newCases = GetNewCaseDataFromDataSet(newCaseDataSet.Tables[0]);

                    var returnList = new List<DailyTestData>();
                    foreach (var testCaseData in latestTestCases)
                    {
                        var casesOnLatestTestDate = newCases.FirstOrDefault(nc => nc.Date == testCaseData.Item1);
                        var positivityRate = (decimal)casesOnLatestTestDate.NewCases / testCaseData.Item2;
                        returnList.Add(new DailyTestData(testCaseData.Item1, testCaseData.Item2, positivityRate));
                    }
                    return new ServiceResponse<DailyTestData[]>(returnList.ToArray());
                }
                catch (Exception ex)
                {
                    return new ServiceResponse<DailyTestData[]>(
                        "An error occurred loading the test data from the State of Texas", ex);
                }
            }
            );
        }

        public async Task<ServiceResponse<DailyHospitalizationRecord[]>> GetLastestHospitalizationCount(int numDays)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var hospitalizationsDataSet = LoadExcelDataAsDataSet("CombinedHospitalDataoverTimebyTSARegion.xlsx");
                    var hospitalizationRecords = GetHospitalizationRecords(hospitalizationsDataSet.Tables).OrderByDescending(r => r.Date);
                    var latestRecords = hospitalizationRecords.Take(numDays);

                    return new ServiceResponse<DailyHospitalizationRecord[]>(latestRecords.ToArray());
                }
                catch (Exception ex)
                {
                    return new ServiceResponse<DailyHospitalizationRecord[]>(
                        "An error occurred loading the hospital data from the State of Texas", ex);
                }
            }
            );
        }

        public async Task<ServiceResponse<DailyDeathRecord[]>> GetLatestDeathCount(int numDays)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var deathDataSet = LoadExcelDataAsDataSet("TexasCOVID19FatalityCountDatabyCounty.xlsx");
                    var deathRecords = GetDeathRecords(deathDataSet.Tables[0]).OrderByDescending(r => r.Date);
                    var latestRecord = deathRecords.Take(numDays);

                    return new ServiceResponse<DailyDeathRecord[]>(latestRecord.ToArray());
                }
                catch (Exception ex)
                {
                    return new ServiceResponse<DailyDeathRecord[]>(
                        "An error occurred loading the fatality data from the State of Texas", ex);
                }
            }
            );
        }

        #endregion

        #region Private Method

        private DataSet LoadExcelDataAsDataSet(string excelFile)
        {
            var excelStream = GetExcelFileStream(excelFile);
            using var excelReader = ExcelReaderFactory.CreateReader(excelStream);
            return excelReader.AsDataSet();
        }

        private Stream GetExcelFileStream(string filename)
        {

            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            return assembly.GetManifestResourceStream($"Services.StateOfTexas.Data.{filename}");
        }

        private List<NewCaseRecord> GetNewCaseDataFromDataSet(DataTable newCaseData)
        {
            var columnCount = newCaseData.Columns.Count;
            var returnList = new List<NewCaseRecord>();
            for (int colIndex = 1; colIndex < columnCount; colIndex++)
            {
                var date = ParseNewCaseDate(newCaseData.Rows[0][colIndex].ToString());
                var newCaseCount = ParseNewCaseCount(newCaseData.Rows[1][colIndex].ToString());

                returnList.Add(new NewCaseRecord(newCaseCount,date));
            }

            return returnList;
        }

        private List<Tuple<DateTime, int>> GetDailyTestCaseCount(DataTable testData)
        {
            var columnCount = testData.Columns.Count;
            var returnList = new List<Tuple<DateTime, int>>();
            var prevTestCount = 0;
            for (int colIndex = 1; colIndex < columnCount; colIndex++)
            {
                var date = ParseTestDataDate(testData.Rows[0][colIndex].ToString());
                var testCount = ParseTestCount(testData.Rows[1][colIndex].ToString());
                var dailyTestCount = testCount - prevTestCount;
                prevTestCount = testCount;

                returnList.Add(new Tuple<DateTime, int>(date, dailyTestCount));
            }

            return returnList;
        }

        private List<DailyHospitalizationRecord> GetHospitalizationRecords(DataTableCollection hospitalizationData)
        {
            var hospitalCountTable = hospitalizationData[0];
            var covidPctOfCapacityTable = hospitalizationData[1];

            var columnCount = hospitalCountTable.Columns.Count;
            var returnList = new List<DailyHospitalizationRecord>();
            var prevHospitalizationCount = 0;
            for (int colIndex = 2; colIndex < columnCount; colIndex++)
            {
                var date = ParseHospitalDataDate(hospitalCountTable.Rows[0][colIndex].ToString());
                var hospitalizationCount = ParseHospitalizationCount(hospitalCountTable.Rows[1][colIndex].ToString());
                var dailyHospitalizationCount = hospitalizationCount - prevHospitalizationCount;
                prevHospitalizationCount = hospitalizationCount;

                var covidPctOfHospitalizations = ParseHospitalizationPct(covidPctOfCapacityTable.Rows[1][colIndex].ToString());

                returnList.Add(new DailyHospitalizationRecord(date, dailyHospitalizationCount, hospitalizationCount, 
                    covidPctOfHospitalizations));
            }

            return returnList;
        }

        private List<DailyDeathRecord> GetDeathRecords(DataTable deathData)
        {
            var columnCount = deathData.Columns.Count;
            var returnList = new List<DailyDeathRecord>();
            var prevDeathCount = 0;
            for (int colIndex = 2; colIndex < columnCount; colIndex++)
            {
                var date = ParseDeathDataDate(deathData.Rows[0][colIndex].ToString());
                var deathCount = ParseDeathCount(deathData.Rows[1][colIndex].ToString());
                var dailyDeathCount = deathCount - prevDeathCount;
                prevDeathCount = deathCount;

                returnList.Add(new DailyDeathRecord(date, dailyDeathCount, deathCount));
            }

            return returnList;
        }

        private List<DailyCovidHospitalizationPctRecord> GetCovidHopitalizationPctRecords(DataTable dataTable)
        {
            var columnCount = dataTable.Columns.Count;
            var returnList = new List<DailyCovidHospitalizationPctRecord>();
            for (int colIndex = 2; colIndex < columnCount; colIndex++)
            {
                var date = ParseHospitalDataDate(dataTable.Rows[0][colIndex].ToString());
                var pct = ParseHospitalizationPct(dataTable.Rows[1][colIndex].ToString());

                returnList.Add(new DailyCovidHospitalizationPctRecord(pct, date));
            }

            return returnList;
        }

        private DateTime ParseNewCaseDate(string dateString)
        {
            dateString = dateString.Replace("New Cases ", string.Empty);

            var splitDate = dateString.Split("-");
            var hasYear = splitDate.Length == 3;
            var year = hasYear ? int.Parse(splitDate[0]) : 2020;
            var month = int.Parse(splitDate[hasYear ? 1 : 0]);
            var day = int.Parse(splitDate[hasYear ? 2 : 1]);

            return new DateTime(year, month, day);
        }

        private DateTime ParseTestDataDate(string dateString)
        {
            return DateTime.Parse(dateString);
        }

        private DateTime ParseHospitalDataDate(string dateString)
        {
            return DateTime.Parse(dateString);
        }

        private DateTime ParseDeathDataDate(string dateString)
        {
            dateString = dateString.Replace("Fatalities ", string.Empty);

            var splitDate = dateString.Split("-");
            var hasYear = splitDate.Length == 3;
            var year = hasYear ? int.Parse(splitDate[0]) : 2020;
            var month = int.Parse(splitDate[hasYear ? 1 : 0]);
            var day = int.Parse(splitDate[hasYear ? 2 : 1]);

            return new DateTime(year, month, day);
        }

        private int ParseNewCaseCount(string newCaseCountString)
        {
            return int.Parse(newCaseCountString);
        }

        private int ParseTestCount(string testCountString)
        {
            var filteredString= testCountString.Replace(",", string.Empty);
            return int.Parse(filteredString);
        }

        private int ParseHospitalizationCount(string hospitalizationCountString)
        {
            return int.Parse(hospitalizationCountString);
        }

        private decimal ParseHospitalizationPct(string dataValue)
        {
            return decimal.Parse(dataValue.Replace("%", string.Empty));
        }


        private int ParseDeathCount(string deathCountString)
        {
            return int.Parse(deathCountString);
        }

        #endregion
    }
}
