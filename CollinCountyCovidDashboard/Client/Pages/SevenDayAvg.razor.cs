using System;
using System.Threading.Tasks;

using Application.Queries.Get7DayAvg;

using Microsoft.AspNetCore.Components;

namespace CollinCountyCovidDashboard.Client.Pages
{
    public partial class SevenDayAvg
    {
        #region Dependencies

        [Inject] protected IGet7DayAvgQuery _get7DayAvgQuery { get; set; }

        #endregion

        #region Properties

        private bool IsLoading => Model == null && Error == null;

        private Application.Queries.Get7DayAvg.SevenDayAvg Model{ get; set; }

        private string Error { get; set; }

        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            var getTodayQueryResult = await _get7DayAvgQuery.Execute();
            if (getTodayQueryResult.WasSuccessful) Model = getTodayQueryResult.Result;
            else Error = getTodayQueryResult.Error;
        }

        #endregion

        #region Methods

        private string GetValueFontSize(int val)
        {
            if (val < 1000) return "10vmax";
            var digitCount = Math.Floor(Math.Log10(val/1000))+1;
            var fontSize = 10 - digitCount*3;
            if (fontSize < 2) return "2vmax";
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
