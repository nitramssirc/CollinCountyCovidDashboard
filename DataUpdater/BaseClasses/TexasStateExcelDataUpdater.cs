using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

using CsvHelper;

using ExcelDataReader;

namespace DataUpdater.BaseClasses
{
    public abstract class TexasStateExcelDataUpdater
    {
        protected abstract string StateOfTexasDataUrl { get; }
        protected abstract string LocalDataName { get; }

        protected abstract int texasDataTableIndex { get; }

        protected abstract int texasDateRowIndex { get; }

        protected abstract int texasValueRowIndex { get; }

        public async Task Execute()
        {
            try
            {
                Console.WriteLine($"Updating {LocalDataName}");
                var texasStateData = await DownloadDataSet();
                var texasStateTable = texasStateData.Tables[texasDataTableIndex];

                var localTable = GetLocalDataTable();

                if (!HasUpdate(texasStateTable, localTable))
                {
                    Console.WriteLine("    New data has not been posted.  Moving on");
                    return;
                }

                var latestColumnCount = texasStateTable.Columns.Count - 1;
                Func<int> localColumnCount = () => localTable.Columns.Count - 1;

                while (localColumnCount() < latestColumnCount)
                {
                    var newColIndex = localColumnCount() + 1;
                    var latestTexasStateDate = texasStateTable.Rows[texasDateRowIndex][newColIndex].ToString();
                    var latestNewCaseCount = texasStateTable.Rows[texasValueRowIndex][newColIndex].ToString();

                    Console.WriteLine("    Adding New Record");
                    Console.WriteLine($"    -Date: {latestTexasStateDate}");
                    Console.WriteLine($"    -Value:{latestNewCaseCount}");

                    localTable.Columns.Add();
                    localTable.Rows[0][localColumnCount()] = latestTexasStateDate;
                    localTable.Rows[1][localColumnCount()] = latestNewCaseCount;
                }

                UpdateLocalData(localTable);
            }
            finally
            {
                Console.WriteLine(" ");
            }
        }

        private bool HasUpdate(DataTable texasStateTable, DataTable localTable)
        {
            return texasStateTable.Columns.Count > localTable.Columns.Count;
        }

        protected string LocalDataFilePath
        {
            get
            {
                return Path.GetFullPath($@"{Assembly.GetExecutingAssembly().Location}\..\..\..\..\..\Services\StateOfTexas\Data\{LocalDataName}.csv");
            }
        }

        protected async Task<DataSet> DownloadDataSet()
        {
            var webClient = new WebClient();
            var data = await webClient.DownloadDataTaskAsync(new Uri(StateOfTexasDataUrl));
            var stream = new MemoryStream(data);
            return GetExcelDataSetFromStream(stream);
        }

        protected DataTable GetLocalDataTable()
        {
            var stream = File.OpenRead(LocalDataFilePath);
            return LoadCSVDataAsDataTable(stream);
        }

        private DataSet GetExcelDataSetFromStream(Stream stream)
        {
            var excelReader = ExcelReaderFactory.CreateReader(stream);
            return excelReader.AsDataSet();
        }

        private DataTable LoadCSVDataAsDataTable(Stream stream)
        {
            var data = new DataTable();
            var createColumns = true;
            using var reader = new StreamReader(stream);
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

        protected void UpdateLocalData(DataTable updatedData)
        {
            SaveDataTableToCSV(updatedData, LocalDataFilePath);
        }

        private void SaveDataTableToCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
    }
}
