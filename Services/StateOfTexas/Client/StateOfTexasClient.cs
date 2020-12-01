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
        private static readonly HttpClient _httpClient = new HttpClient();

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
            Console.WriteLine(string.Join(',', resourceNames));
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

        private int ParseNewCaseCount(string newCaseCountString)
        {
            return int.Parse(newCaseCountString);
        }

        private List<DateTime> GetDates(DataRow dataRow, int columnCount)
        {
            var dateList = new List<DateTime>();
            for (int i = 1; i < columnCount; i++)
            {
                var dateString = dataRow[i].ToString();
                dateList.Add(DateTime.Parse(dateString));
            }
            return dateList;
        }

        private List<int> GetTestingTotals(DataRow dataRow, int columnCount)
        {
            var dataList = new List<int> { 0 };
            var prevAmt = int.Parse(dataRow[1].ToString());
            for (int i = 2; i < columnCount; i++)
            {
                var amt = int.Parse(dataRow[i].ToString());
                dataList.Add(amt - prevAmt);
                prevAmt = amt;
            }
            return dataList;
        }

        private CumulativeTestsOverTime GenerateCumulativeTestsOverTime(List<DateTime> dates, List<int> testingTotals)
        {
            var dailyTestData = dates.Select((date, i) => new DailyTestData(date, testingTotals[i]));
            return new CumulativeTestsOverTime(dailyTestData.ToArray());
        }


        #endregion
    }
}
