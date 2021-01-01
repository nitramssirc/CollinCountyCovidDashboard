using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CsvHelper;

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
                    var newCaseDataSet = LoadCSVDataAsDataTable("TexasCOVID19NewConfirmedCasesbyCounty.csv");
                    var newCases = GetNewCaseDataFromDataSet(newCaseDataSet).OrderByDescending(r => r.Date);

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
                    var newCaseDataSet = LoadCSVDataAsDataTable("TexasCOVID19NewConfirmedCasesbyCounty.csv");
                    var newCases = GetNewCaseDataFromDataSet(newCaseDataSet);
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
                    var testDataSet = LoadCSVDataAsDataTable("CumulativeTestsOverTimeByCounty.csv");
                    var testData = GetDailyTestCaseCount(testDataSet).OrderByDescending(t => t.Item1);
                    var latestTestCases = testData.Take(numDays);

                    var newCaseDataSet = LoadCSVDataAsDataTable("TexasCOVID19NewConfirmedCasesbyCounty.csv");
                    var newCases = GetNewCaseDataFromDataSet(newCaseDataSet);

                    var returnList = new List<DailyTestData>();
                    foreach (var testCaseData in latestTestCases)
                    {
                        var date = testCaseData.Item1;
                        var tests = testCaseData.Item2;
                        var casesOnLatestTestDate = newCases.FirstOrDefault(nc => nc.Date == date);
                        var positivityRate = tests == 0 || casesOnLatestTestDate == null ? 0 : (decimal)casesOnLatestTestDate.NewCases / tests;
                        returnList.Add(new DailyTestData(date, tests, positivityRate));
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
                    var hospitalizationsTable = LoadCSVDataAsDataTable("CombinedHospitalDataoverTimebyTSARegion_CovidHospitialization.csv");
                    var covidPctCapacityTable = LoadCSVDataAsDataTable("CombinedHospitalDataoverTimebyTSARegion_CovidPctCapacity.csv");
                    var hospitalizationRecords = GetHospitalizationRecords(hospitalizationsTable, covidPctCapacityTable).OrderByDescending(r => r.Date);
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
                    var deathDataSet = LoadCSVDataAsDataTable("TexasCOVID19FatalityCountDatabyCounty.csv");
                    var deathRecords = GetDeathRecords(deathDataSet).OrderByDescending(r => r.Date);
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

        public async Task<ServiceResponse<DailyHospitalBedRecord[]>> GetHospitalBedRecords(int numDays)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var availableBedsTable = LoadCSVDataAsDataTable("CombinedHospitalDataoverTimebyTSARegion_TotalAvailableBeds.csv");
                    var occupiedBedsTable = LoadCSVDataAsDataTable("CombinedHospitalDataoverTimebyTSARegion_TotalOccupiedBeds.csv");
                    var covidHospitalizations = LoadCSVDataAsDataTable("CombinedHospitalDataoverTimebyTSARegion_CovidHospitialization.csv");

                    var bedRecords = ConstructHospitalBedRecords(availableBedsTable, occupiedBedsTable, covidHospitalizations);

                    return new ServiceResponse<DailyHospitalBedRecord[]>(bedRecords.OrderByDescending(r => r.Date).Take(numDays).ToArray());
                }
                catch (Exception ex)
                {
                    return new ServiceResponse<DailyHospitalBedRecord[]>(
                        "An error occurred loading the hospital bed records from the State of Texas", ex);
                }
            });
        }

        #endregion

        #region Private Method

        private DataTable LoadCSVDataAsDataTable(string csvFile)
        {
            var data = new DataTable();
            var createColumns = true;
            using var reader = new StreamReader(GetManifestDataFileStream(csvFile));
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                while (csv.Read())
                {
                    if (createColumns)
                    {
                        for (int i = 0; i < csv.Context.Record.Length; i++)
                            data.Columns.Add(i.ToString());
                        createColumns = false;
                    }

                    DataRow row = data.NewRow();
                    for (int i = 0; i < csv.Context.Record.Length; i++)
                        row[i] = csv.Context.Record[i];
                    data.Rows.Add(row);
                }
            return data;
        }

        private DataSet LoadExcelDataAsDataSet(string excelFile)
        {
            var excelStream = GetManifestDataFileStream(excelFile);
            using var excelReader = ExcelReaderFactory.CreateReader(excelStream);
            return excelReader.AsDataSet();
        }

        private Stream GetManifestDataFileStream(string filename)
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

                returnList.Add(new NewCaseRecord(newCaseCount, date));
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

        private List<DailyHospitalizationRecord> GetHospitalizationRecords(DataTable hospitalCountTable, DataTable covidPctOfCapacityTable)
        {
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

        private List<DailyHospitalBedRecord> ConstructHospitalBedRecords(DataTable availableBedsTable, DataTable occupiedBedsTable, DataTable covidHospitalizations)
        {
            var columnCount = covidHospitalizations.Columns.Count;
            var returnList = new List<DailyHospitalBedRecord>();

            for (int colIndex = 2; colIndex < columnCount; colIndex++)
            {
                var date = ParseHospitalDataDate(covidHospitalizations.Rows[0][colIndex].ToString());
                var covidHospitalizationCount = ParseHospitalizationCount(covidHospitalizations.Rows[1][colIndex].ToString());
                var availableBedCount = ParseHospitalizationCount(availableBedsTable.Rows[1][colIndex].ToString());
                var occupiedBedCount = ParseHospitalizationCount(occupiedBedsTable.Rows[1][colIndex].ToString());

                returnList.Add(new DailyHospitalBedRecord(date, availableBedCount, occupiedBedCount, covidHospitalizationCount));
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
            var filteredString = testCountString.Replace(",", string.Empty);
            return int.Parse(filteredString);
        }

        private int ParseHospitalizationCount(string hospitalizationCountString)
        {
            return int.Parse(hospitalizationCountString);
        }

        private decimal ParseHospitalizationPct(string dataValue)
        {
            var hasPct = dataValue.Contains("%");
            return hasPct
                ? decimal.Parse(dataValue.Replace("%", string.Empty)) / 100
                : decimal.Parse(dataValue);
        }

        private int ParseDeathCount(string deathCountString)
        {
            return int.Parse(deathCountString);
        }


        #endregion
    }
}
