using System;
using System.Data;
using System.Threading.Tasks;

using DataUpdater.BaseClasses;
using DataUpdater.Interfaces;

namespace DataUpdater.DataUpdaters
{
    public class TestDataUpdater : TexasStateExcelDataUpdater, IDataUpdater
    {
        #region TexasStateExcelDataUpdater

        protected override string StateOfTexasDataUrl => @"https://dshs.texas.gov/coronavirus/TexasCOVID-19CumulativeTestsbyCounty.xlsx";

        protected override string LocalDataName => "CumulativeTestsOverTimeByCounty";

        protected override int texasDataTableIndex => 0;

        protected override int texasDateRowIndex => 1;

        protected override int texasValueRowIndex => 44;

        #endregion

    }
}
