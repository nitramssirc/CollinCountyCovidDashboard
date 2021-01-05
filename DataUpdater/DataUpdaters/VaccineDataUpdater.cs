using CsvHelper;

using DataUpdater.Interfaces;

using ExcelDataReader;

using Services.StateOfTexas.Models;

using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace DataUpdater.DataUpdaters
{
    public class VaccineDataUpdater : IDataUpdater
    {
        #region Fields

        string _localCSVFilePath = Path.GetFullPath($@"{Assembly.GetExecutingAssembly().Location}\..\..\..\..\..\Services\StateOfTexas\Data\COVID19VaccineDataByCounty.csv");
        string _localXlxsFilePath = Path.GetFullPath($@"{Assembly.GetExecutingAssembly().Location}\..\..\..\..\..\Services\StateOfTexas\Data\COVID-19 Vaccine Data by County.xlsx");
        Uri _dshsUrl = new Uri(@"https://dshs.texas.gov/immunize/covid19/COVID-19-Vaccine-Data-by-County.xls");

        #endregion

        #region IDataUpdater Implementation

        #endregion
        public async Task Execute()
        {
            Console.WriteLine("Updating Vaccine Data");
            var localData = LoadLocalData();
            var lastestDSHSData = await DownloadLatestDataFromDSHS();
            
            if (localData.Equals(lastestDSHSData))
            {
                Console.WriteLine("    New data has not been posted.  Moving on");
                return;
            }

            Console.WriteLine(lastestDSHSData);
            UpdateLocalData(lastestDSHSData);
        }

        #region Private Methods

        private DailyVaccineDataRecord LoadLocalData()
        {
            using var reader = new StreamReader(_localCSVFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<DailyVaccineDataRecord>();
            return records.Last();
        }

        private async Task<DailyVaccineDataRecord> DownloadLatestDataFromDSHS()
        {
            var webClient = new WebClient();
            var data = await webClient.DownloadDataTaskAsync(_dshsUrl);
            await File.WriteAllBytesAsync(_localXlxsFilePath, data);
            var stream = new MemoryStream(data);
            var dataSet = GetExcelDataSetFromStream(stream);
            return ConstructVaccineDataRecordFromXlsxDataSet(dataSet);
        }

        private DailyVaccineDataRecord ConstructVaccineDataRecordFromXlsxDataSet(DataSet dataSet)
        {
            var table = dataSet.Tables[1];
            var row = table.Rows[44];
            return new DailyVaccineDataRecord()
            {
                Date = DateTime.Today,
                VaccineDosesDistributed = int.Parse(row[2].ToString()),
                VaccineDosesAdministered = int.Parse(row[3].ToString()),
                PeopleVaccinatedWithAtLeastOneDose = int.Parse(row[4].ToString()),
                PeopleFullyVaccinated = int.Parse(row[5].ToString()),
                Population16Plus = int.Parse(row[6].ToString()),
                Population65Plus = int.Parse(row[7].ToString()),
                Phase1AHeathcareWorkers = int.Parse(row[8].ToString()),
                Phase1ALongTermCareResidents = int.Parse(row[9].ToString())
            };
        }

        private DataSet GetExcelDataSetFromStream(Stream stream)
        {
            var excelReader = ExcelReaderFactory.CreateReader(stream);
            return excelReader.AsDataSet();
        }

        private void UpdateLocalData(DailyVaccineDataRecord lastestDSHSData)
        {
            using (var stream = File.Open(_localCSVFilePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Don't write the header again.
                csv.Configuration.HasHeaderRecord = false;
                csv.NextRecord();
                csv.WriteRecord(lastestDSHSData);
            }
        }



        #endregion
    }
}
