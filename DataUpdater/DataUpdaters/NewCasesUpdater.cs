using System;
using System.Data;
using System.Threading.Tasks;

using DataUpdater.BaseClasses;
using DataUpdater.Interfaces;

namespace DataUpdater.DataUpdaters
{
    public class NewCasesUpdater : TexasStateExcelDataUpdater, IDataUpdater
    {
        #region TexasStateExcelDataUpdater

        protected override string StateOfTexasDataUrl => @"https://dshs.texas.gov/coronavirus/TexasCOVID-19NewCasesOverTimebyCounty.xlsx";

        protected override string LocalDataName => "TexasCOVID19NewConfirmedCasesbyCounty";

        #endregion

        #region IDataUpdater Implementation

        public async Task Execute()
        {
            Console.WriteLine("Updating New Case Data");
            var texasStateData = await DownloadDataSet();
            var texasStateTable = texasStateData.Tables[0];

            var localTable = GetLocalDataTable();

            if (!HasUpdate(texasStateTable, localTable))
            {
                Console.WriteLine("    New data has not been posted.  Moving on");
                return;
            }

            var columnCount = texasStateTable.Columns.Count - 1;
            var latestTexasStateDate = texasStateTable.Rows[2][columnCount].ToString();
            var latestNewCaseCount = texasStateTable.Rows[46][columnCount].ToString();

            Console.WriteLine("    Adding New Record");
            Console.WriteLine($"    -Date: {latestTexasStateDate}");
            Console.WriteLine($"    -New Cases:{latestNewCaseCount}");

            localTable.Columns.Add();
            columnCount++;

            localTable.Rows[0][columnCount] = latestTexasStateDate;
            localTable.Rows[1][columnCount] = latestNewCaseCount;

            UpdateLocalData(localTable);
        }

        #endregion

        #region Private Methods

        private bool HasUpdate(DataTable texasStateTable, DataTable localTable)
        {
            return texasStateTable.Columns.Count > localTable.Columns.Count;
        }

        private DateTime ParseDate(string dateString)
        {
            return DateTime.Parse(dateString);
        }

        private int ParseNewCaseCount(string newCaseCountString)
        {
            return int.Parse(newCaseCountString);
        }

        #endregion
    }
}
