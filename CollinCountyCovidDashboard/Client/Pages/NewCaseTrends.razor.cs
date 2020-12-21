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
        TrendChart<int> _trendChart;

        [Inject] IGetNewCasesQuery _getNewCasesQuery { get; set; }



        #region Overrides

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var queryResults = await _getNewCasesQuery.Execute(30);
                await _trendChart.SetChartData(ConstructTrendChartRecords(queryResults));
            }
        }

        #endregion

        private TrendChartRecord<int>[] ConstructTrendChartRecords(QueryResult<NewCaseModel[]> queryResults)
        {
            return queryResults.Result.Select(r => new TrendChartRecord<int>(r.Date, r.NewCases, (int)Math.Round(r.SevenDayAvg))).ToArray();
        }
    }
}
