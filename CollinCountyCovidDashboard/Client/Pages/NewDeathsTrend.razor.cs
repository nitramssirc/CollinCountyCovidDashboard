using Application.BaseModels;
using Application.Queries.GetDeaths;
using Application.Queries.GetNewCases;

using CollinCountyCovidDashboard.Client.Models;
using CollinCountyCovidDashboard.Client.Shared;

using Microsoft.AspNetCore.Components;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client.Pages
{
    public partial class NewDeathsTrend
    {
        TrendChart<decimal> _trendChart;

        [Inject] IGetDeathsQuery _getDeathsQuery { get; set; }



        #region Overrides

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var queryResults = await _getDeathsQuery.Execute(30);
                await _trendChart.SetChartData(ConstructTrendChartRecords(queryResults));
            }
        }

        #endregion

        private TrendChartRecord<decimal>[] ConstructTrendChartRecords(QueryResult<DailyDeathModel[]> queryResults)
        {
            return queryResults.Result.Select(r => new TrendChartRecord<decimal>(r.Date, r.Deaths, r.SevenDayAvg)).ToArray();
        }
    }
}
