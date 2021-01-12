using CsvHelper;

using DataUpdater.Interfaces;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataUpdater.DataUpdaters
{
    public class VaccineProviderDataUpdater : IDataUpdater
    {
        #region Fields

        private readonly string _localRawFilename = Path.GetFullPath($@"{Assembly.GetExecutingAssembly().Location}\..\..\..\..\..\Services\StateOfTexas\Data\VaccineProviderAccessibilityData.csv");
        private readonly string _localFilteredFilename = Path.GetFullPath($@"{Assembly.GetExecutingAssembly().Location}\..\..\..\..\..\Services\StateOfTexas\Data\VaccineProviderAccessibilityData_CollinCounty.csv");
        private readonly Uri _dshsUrl = new Uri("https://genesis.soc.texas.gov/files/accessibility/vaccineprovideraccessibilitydata.csv");

        #endregion

        #region IDataUpdater Implementation

        public async Task Execute()
        {
            Console.WriteLine("Updating Vaccine Provider Data");

            var currentRawData = LoadCurrentRawData();
            var latestDownloadedData = await DownloadLatestData();

            if(currentRawData == LoadCurrentRawData())
            {
                Console.WriteLine("    New data has not been posted.  Moving on");
                return;
            }

            var header = latestDownloadedData.Rows[0];
            var collinCountryProviderData = FilterCollinCountryData(latestDownloadedData);
            SaveFilteredData(header, collinCountryProviderData);                
        }

        #endregion

        #region Private Methods

        private string LoadCurrentRawData()
        {
            return File.ReadAllText(_localRawFilename);
        }

        private async Task<DataTable> DownloadLatestData()
        {
            var webClient = new WebClient();
            var data = await webClient.DownloadDataTaskAsync(_dshsUrl);
            await File.WriteAllBytesAsync(_localRawFilename, data);
            return LoadLocalRawDataAsDataTable();
        }

        private DataTable LoadLocalRawDataAsDataTable()
        {
            var data = new DataTable();
            var createColumns = true;
            using var reader = new StreamReader(_localRawFilename);
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

        private IEnumerable<DataRow> FilterCollinCountryData(DataTable latestDownloadedData)
        {
            for (int rowIndex = 0; rowIndex < latestDownloadedData.Rows.Count; rowIndex++)
            {
                var row = latestDownloadedData.Rows[rowIndex];
                var county = row[5].ToString();
                if (county.Equals("Collin", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"    {row[0].ToString()}");
                    yield return row;
                }
            }
        }

        private void SaveFilteredData(DataRow header, IEnumerable<DataRow> collinCountryProviderData)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(GetCSVStringFromDataRow(header));
            foreach (var ccProviderRow in collinCountryProviderData)
            {
                stringBuilder.AppendLine(GetCSVStringFromDataRow(ccProviderRow));
            }

            using var writer = new StreamWriter(_localFilteredFilename, append: false);
            writer.Write(stringBuilder);
        }

        private string GetCSVStringFromDataRow(DataRow row)
        {
            var columnData = new List<string>();
            for (int colIndex = 0; colIndex < row.Table.Columns.Count; colIndex++)
            {
                columnData.Add(row[colIndex].ToString());
            }
            return string.Join(',', columnData);
        }



        #endregion

    }
}
