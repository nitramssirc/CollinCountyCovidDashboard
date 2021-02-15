using Application.Queries.GetVaccineData;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client.Pages
{
    public partial class VaccineInfo
    {
        #region Dependencies

        [Inject] protected IGetVaccineDataQuery _getVaccineDataQuery { get; set; }

        #endregion

        #region Properties

        private bool IsLoading => Model == null && Error == null;

        private VaccineDataModel Model { get; set; }

        private string Error { get; set; }

        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            var getTodayQueryResult = await _getVaccineDataQuery.Execute();
            if (getTodayQueryResult.WasSuccessful) Model = getTodayQueryResult.Result;
            else Error = getTodayQueryResult.Error;
        }

        #endregion

        #region Methods

        private string GetValueFontSize(int val)
        {
            if (val < 1000) return "10vmax";
            var digitCount = Math.Floor(Math.Log10(val / 1000)) + 1;
            var fontSize = 10 - digitCount * 2;
            Math.Clamp(fontSize, 2, 10);
            return $"{fontSize}vmax";
        }

        private string GetValueFontSize(decimal val)
        {
            val *= 10;
            if (val < 1000) return "10vmax";
            var digitCount = Math.Floor(Math.Log10((double)(val / 1000))) + 1;
            var fontSize = 10 - digitCount * 3;
            if (fontSize < 2) return "2vmax";
            return $"{fontSize}vmax";
        }

        #endregion

    }
}
