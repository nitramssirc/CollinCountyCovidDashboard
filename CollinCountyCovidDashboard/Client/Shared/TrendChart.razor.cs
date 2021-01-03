using Blazorise.Charts;

using CollinCountyCovidDashboard.Client.Models;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client.Shared
{
    public partial class TrendChart<ChartDataType>
    {
        #region Parameters

        [Parameter]
        public string DailyRecordLabel { get; set; }

        #endregion

        #region Fields

        LineChart<ChartDataType> lineChart;

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
                    fontColor = "#fff"
                }
            },
            responsive = true,
            maintainAspectRatio = false,
            animation = new
            {
                duration=0
            }
        };

        #endregion

        #region Public Methods

        public async Task SetChartData(TrendChartRecord<ChartDataType>[] chartData)
        {
            await lineChart.Clear();
            await lineChart.AddLabelsDatasetsAndUpdate(
                GetDateLabels(chartData),
                Get7DayAverageDataSet(chartData),
                GetNewCaseCountDataSet(chartData));
        }

        #endregion

        #region Private Methods

        string[] GetDateLabels(TrendChartRecord<ChartDataType>[] records)
        {
            return records.Select(d => d.Date.ToShortDateString()).ToArray();
        }

        LineChartDataset<ChartDataType> GetNewCaseCountDataSet(TrendChartRecord<ChartDataType>[] records)
        {
            return new LineChartDataset<ChartDataType>
            {
                Data = records.Select(r => r.DailyValue).ToList(),
                BackgroundColor = new List<string> { ChartColor.FromRgba(255, 99, 132, 0.2f) },
                BorderColor = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f) },
                Fill = true,
                SteppedLine = true,
                Label = DailyRecordLabel,
                PointRadius = 2,
                PointBorderColor = Enumerable.Repeat("#ff6384", records.Length).ToList(),
                PointBackgroundColor = Enumerable.Repeat("#ff6384", records.Length).ToList()
            };
        }

        LineChartDataset<ChartDataType> Get7DayAverageDataSet(TrendChartRecord<ChartDataType>[] result)
        {
            return new LineChartDataset<ChartDataType>
            {
                Data = result.Select(r => r.SevenDayAvg).ToList(),
                BorderColor = new List<string> { ChartColor.FromRgba(255, 255, 255, 1f) },
                Fill = false,
                PointRadius = 2,
                BorderDash = new List<int> { },
                SteppedLine = false,
                Label = "7 Day Average",
                PointBorderColor = Enumerable.Repeat("#fff", result.Length).ToList(),
                PointBackgroundColor = Enumerable.Repeat("#fff", result.Length).ToList()
            };
        }

        #endregion

    }
}
