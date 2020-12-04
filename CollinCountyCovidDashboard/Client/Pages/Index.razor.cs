using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.Queries.GetToday;

using Microsoft.AspNetCore.Components;

using Services.Common;
using Services.StateOfTexas.Client;
using Services.StateOfTexas.Models;

namespace CollinCountyCovidDashboard.Client.Pages
{
    public partial class Index
    {
        #region Dependencies

        [Inject] protected IGetTodayQuery _getTodayQuery { get; set; }

        #endregion

        #region Properties

        private bool IsLoading => Today == null && Error == null;

        private Today Today { get; set; }

        private string Error { get; set; }

        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            var getTodayQueryResult = await _getTodayQuery.Execute();
            if (getTodayQueryResult.WasSuccessful) Today = getTodayQueryResult.Result;
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

        #endregion
    }
}
