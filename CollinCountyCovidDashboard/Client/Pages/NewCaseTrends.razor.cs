using Application.Queries.GetNewCases;

using Blazorise.Charts;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client.Pages
{
    public partial class NewCaseTrends
    {
        LineChart<int> lineChart;

        object chartOptions = new
        {
            scales = new
            {
                xAxes = new object[]
                {
                    new
                    {
                        ticks=new
                        {
                            fontColor = "#fff"
                        }
                    }
                },
                yAxes = new object[]
                {
                    new
                    {
                        ticks=new
                        {
                            fontColor = "#fff"
                        }
                    }
                },
            },
            legend = new
            {
                labels = new
                {
                    fontColor="#fff"
                }
            },
            responsive = true,
            maintainAspectRatio = false,
        };

        [Inject] IGetNewCasesQuery _getNewCasesQuery { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await HandleRedraw();
            }
        }

        async Task HandleRedraw()
        {
            await lineChart.Clear();
            var newCasesData = await _getNewCasesQuery.Execute(30);
            await lineChart.AddLabelsDatasetsAndUpdate(
                GetDateLabels(newCasesData.Result),
                Get7DayAverageDataSet(newCasesData.Result),
                GetNewCaseCountDataSet(newCasesData.Result));
        }

        string[] GetDateLabels(NewCaseModel[] result)
        {
            return result.Select(d => d.Date.ToShortDateString()).ToArray();
        }

        LineChartDataset<int> GetNewCaseCountDataSet(NewCaseModel[] result)
        {
            return new LineChartDataset<int>
            {                
                Data = result.Select(r=>r.NewCases).ToList(),
                BackgroundColor = new List<string> { ChartColor.FromRgba(255, 99, 132, 0.2f) },
                BorderColor = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f) },
                Fill = true,
                SteppedLine = true,
                Label = "New Confirmed Cases",
                PointRadius = 4,
                PointBorderColor = Enumerable.Repeat("#ff6384", result.Length).ToList(),
                PointBackgroundColor = Enumerable.Repeat("#ff6384", result.Length).ToList()
            };
        }

        LineChartDataset<int> Get7DayAverageDataSet(NewCaseModel[] result)
        {
            return new LineChartDataset<int>
            {
                Data = result.Select(r => (int)Math.Round(r.SevenDayAvg)).ToList(),
                BorderColor = new List<string> { ChartColor.FromRgba(255, 255, 255, 1f) },
                Fill = false,
                PointRadius = 4,
                BorderDash = new List<int> { },
                SteppedLine = false,
                Label = "7 Day Average",
                PointBorderColor = Enumerable.Repeat("#fff", result.Length).ToList(),
                PointBackgroundColor = Enumerable.Repeat("#fff", result.Length).ToList()
            };
        }

        List<string> backgroundColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 0.2f) };
        List<string> borderColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f) };
        private object r;
    }
}
