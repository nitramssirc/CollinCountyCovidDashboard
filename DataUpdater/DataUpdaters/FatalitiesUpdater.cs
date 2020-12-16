using System;
using System.Data;
using System.Threading.Tasks;

using DataUpdater.BaseClasses;
using DataUpdater.Interfaces;

namespace DataUpdater.DataUpdaters
{
    public class FatalitiesUpdater : TexasStateExcelDataUpdater, IDataUpdater
    {
        #region TexasStateExcelDataUpdater

        protected override string StateOfTexasDataUrl => @"https://dshs.texas.gov/coronavirus/TexasCOVID19DailyCountyFatalityCountData.xlsx";

        protected override string LocalDataName => "TexasCOVID19FatalityCountDatabyCounty";

        protected override int texasDataTableIndex => 0;

        protected override int texasDateRowIndex => 2;

        protected override int texasValueRowIndex => 45;

        #endregion

    }
}
