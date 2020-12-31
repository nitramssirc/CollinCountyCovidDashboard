using Application.BaseModels;
using Application.Queries.GetNewCases;

using Blazorise.Charts;

using CollinCountyCovidDashboard.Client.Models;
using CollinCountyCovidDashboard.Client.Shared;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client.Pages
{
    public partial class NewCaseTrends
    {
        #region Dependencies

        [Inject] IGetNewCasesQuery _getNewCasesQuery { get; set; }

        #endregion

        #region View References

        TrendChart<int> _trendChart;
        NumDaysSlider _numDaysSlider;

        #endregion

        #region View Properties

        private int NumDays
        {
            get; set;
        }

        #endregion

        #region Overrides

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                NumDays = _numDaysSlider.NumDays;
                await LoadChartData();
                StateHasChanged();
            }
        }

        #endregion

        #region Event Handlers

        private async Task OnNumDaysChanged()
        {
            NumDays = _numDaysSlider.NumDays;
            await LoadChartData();
            StateHasChanged();
        }

        #endregion

        #region Private Methods

        private async Task LoadChartData()
        {
            var queryResults = await _getNewCasesQuery.Execute(NumDays);
            await _trendChart.SetChartData(ConstructTrendChartRecords(queryResults));
        }

        private TrendChartRecord<int>[] ConstructTrendChartRecords(QueryResult<NewCaseModel[]> queryResults)
        {
            return queryResults.Result.Select(r => new TrendChartRecord<int>(r.Date, r.NewCases, (int)Math.Round(r.SevenDayAvg))).ToArray();
        }

        #endregion

    }
}
