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

        protected string LocalDataFilePath
        {
            get
            {
                return Path.GetFullPath($@"{Assembly.GetExecutingAssembly().Location}\..\..\..\..\Services\StateOfTexas\Data\{LocalDataName}.csv");
            }
        }

        protected async Task<DataSet> DownloadDataSet()
        {
            var webClient = new WebClient();
            var stream = await webClient.OpenReadTaskAsync(new Uri(StateOfTexasDataUrl));
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
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
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
