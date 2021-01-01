using System;
using System.Data;
using System.Threading.Tasks;

using DataUpdater.BaseClasses;
using DataUpdater.Interfaces;

namespace DataUpdater.DataUpdaters
{
    public class Hospitalizations_CovidHospitalizationsUpdater : TexasStateExcelDataUpdater, IDataUpdater
    {
        #region TexasStateExcelDataUpdater

        protected override string StateOfTexasDataUrl => @"https://dshs.texas.gov/coronavirus/CombinedHospitalDataoverTimebyTSA.xlsx";

        protected override string LocalDataName => "CombinedHospitalDataoverTimebyTSARegion_CovidHospitialization";

        protected override string LocalStateDataName => "CombinedHospitalDataoverTimebyTSARegion";

        protected override int texasDataTableIndex => 0;

        protected override int texasDateRowIndex => 2;

        protected override int texasValueRowIndex => 7;

        #endregion

    }
}
