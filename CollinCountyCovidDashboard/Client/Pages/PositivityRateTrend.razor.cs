using Application.BaseModels;
using Application.Queries.GetPositivityRate;

using CollinCountyCovidDashboard.Client.Models;
using CollinCountyCovidDashboard.Client.Shared;

using Microsoft.AspNetCore.Components;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client.Pages
{
    public partial class PositivityRateTrend
    {
        #region Dependencies

        [Inject] IGetPositivityRateQuery _getPositivityRateQuery { get; set; }

        #endregion

        #region View References

        TrendChart<decimal> _trendChart;
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
            var queryResults = await _getPositivityRateQuery.Execute(NumDays);
            await _trendChart.SetChartData(ConstructTrendChartRecords(queryResults));
        }

        private TrendChartRecord<decimal>[] ConstructTrendChartRecords(QueryResult<DailyPositivityRateModel[]> queryResults)
        {
            return queryResults.Result.Select(r => new TrendChartRecord<decimal>(r.Date, r.PositivityRate * 100, r.SevenDayAvg * 100)).ToArray();
        }

        #endregion

    }

}
