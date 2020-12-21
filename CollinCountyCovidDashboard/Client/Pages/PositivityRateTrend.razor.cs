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
        TrendChart<decimal> _trendChart;

        [Inject] IGetPositivityRateQuery _getPositivityRateQuery { get; set; }



        #region Overrides

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var queryResults = await _getPositivityRateQuery.Execute(30);
                if (!queryResults.WasSuccessful) { Console.WriteLine(queryResults.Error); }
                await _trendChart.SetChartData(ConstructTrendChartRecords(queryResults));
            }
        }

        #endregion

        private TrendChartRecord<decimal>[] ConstructTrendChartRecords(QueryResult<DailyPositivityRateModel[]> queryResults)
        {
            return queryResults.Result.Select(r => new TrendChartRecord<decimal>(r.Date, r.PositivityRate*100, r.SevenDayAvg * 100)).ToArray();
        }
    }
}
