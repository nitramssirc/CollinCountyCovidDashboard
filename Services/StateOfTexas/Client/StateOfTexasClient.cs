using System;
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

        public async Task<ServiceResponse<NewCaseRecord>> GetLatestNewCaseCount()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var newCaseDataSet = LoadExcelDataAsDataSet("TexasCOVID19NewConfirmedCasesbyCounty.xlsx");
                    var newCases = GetNewCaseDataFromDataSet(newCaseDataSet.Tables[0]);
                    var latestDate = newCases.Max(nc => nc.Key);

                    var newCaseRecord = new NewCaseRecord(newCases[latestDate], latestDate);
                    return new ServiceResponse<NewCaseRecord>(newCaseRecord);
                }
                catch (Exception ex)
                {
                    return new ServiceResponse<NewCaseRecord>(
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
                    var totalCases = newCases.Sum(nc => nc.Value);

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

        public async Task<ServiceResponse<DailyTestData>> GetLatestPositiveTestCount()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var testDataSet = LoadExcelDataAsDataSet("CumulativeTestsOverTimeByCounty.xlsx");
                    var testData = GetDailyTestCaseCount(testDataSet.Tables[0]);
                    var latestDate = testData.Max(nc => nc.Key);
                    var latestTestCases = testData[latestDate];

                    var newCaseDataSet = LoadExcelDataAsDataSet("TexasCOVID19NewConfirmedCasesbyCounty.xlsx");
                    var newCases = GetNewCaseDataFromDataSet(newCaseDataSet.Tables[0]);
                    var casesOnLatestTestDate = newCases[latestDate];
                    var positivityRate = (decimal)casesOnLatestTestDate / latestTestCases;
                    var latestTestDataRecord = new DailyTestData(latestDate, latestTestCases, positivityRate);
                    return new ServiceResponse<DailyTestData>(latestTestDataRecord);
                }
                catch (Exception ex)
                {
                    return new ServiceResponse<DailyTestData>(
                        "An error occurred loading the test data from the State of Texas", ex);
                }
            }
            );
        }

        public async Task<ServiceResponse<DailyHospitalizationRecord>> GetLastestHospitalizationCount()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var hospitalizationsDataSet = LoadExcelDataAsDataSet("CombinedHospitalDataoverTimebyTSARegion.xlsx");
                    var hospitalizationRecords = GetHospitalizationRecords(hospitalizationsDataSet.Tables[0]);
                    var latestRecord = hospitalizationRecords.Last();

                    return new ServiceResponse<DailyHospitalizationRecord>(latestRecord);
                }
                catch (Exception ex)
                {
                    return new ServiceResponse<DailyHospitalizationRecord>(
                        "An error occurred loading the hospital data from the State of Texas", ex);
                }
            }
            );
        }

        public async Task<ServiceResponse<DailyDeathRecord>> GetLatestDeathCount()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var deathDataSet = LoadExcelDataAsDataSet("TexasCOVID19FatalityCountDatabyCounty.xlsx");
                    var deathRecords = GetDeathRecords(deathDataSet.Tables[0]);
                    var latestRecord = deathRecords.Last();

                    return new ServiceResponse<DailyDeathRecord>(latestRecord);
                }
                catch (Exception ex)
                {
                    return new ServiceResponse<DailyDeathRecord>(
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

        private Dictionary<DateTime, int> GetNewCaseDataFromDataSet(DataTable newCaseData)
        {
            var columnCount = newCaseData.Columns.Count;
            var returnDictionary = new Dictionary<DateTime, int>();
            for (int colIndex = 1; colIndex < columnCount; colIndex++)
            {
                var date = ParseNewCaseDate(newCaseData.Rows[0][colIndex].ToString());
                var newCaseCount = ParseNewCaseCount(newCaseData.Rows[1][colIndex].ToString());

                returnDictionary.Add(date, newCaseCount);
            }

            return returnDictionary;
        }

        private Dictionary<DateTime, int> GetDailyTestCaseCount(DataTable testData)
        {
            var columnCount = testData.Columns.Count;
            var returnDictionary = new Dictionary<DateTime, int>();
            var prevTestCount = 0;
            for (int colIndex = 1; colIndex < columnCount; colIndex++)
            {
                var date = ParseTestDataDate(testData.Rows[0][colIndex].ToString());
                var testCount = ParseTestCount(testData.Rows[1][colIndex].ToString());
                var dailyTestCount = testCount - prevTestCount;
                prevTestCount = testCount;

                returnDictionary.Add(date, dailyTestCount);
            }

            return returnDictionary;
        }

        private List<DailyHospitalizationRecord> GetHospitalizationRecords(DataTable hospitalizationData)
        {
            var columnCount = hospitalizationData.Columns.Count;
            var returnList = new List<DailyHospitalizationRecord>();
            var prevHospitalizationCount = 0;
            for (int colIndex = 2; colIndex < columnCount; colIndex++)
            {
                var date = ParseHospitalDataDate(hospitalizationData.Rows[0][colIndex].ToString());
                var hospitalizationCount = ParseHospitalizationCount(hospitalizationData.Rows[1][colIndex].ToString());
                var dailyHospitalizationCount = hospitalizationCount - prevHospitalizationCount;
                prevHospitalizationCount = hospitalizationCount;

                returnList.Add(new DailyHospitalizationRecord(date, dailyHospitalizationCount, hospitalizationCount));
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
            if (dateString == "39668") return new DateTime(2020, 08, 08);
            if (dateString == "44059") return new DateTime(2020, 08, 16);
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

        private int ParseDeathCount(string deathCountString)
        {
            return int.Parse(deathCountString);
        }

        #endregion
    }
}
