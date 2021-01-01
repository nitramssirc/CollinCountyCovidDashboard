using Application.BaseModels;
using Application.Queries.GetHospitalBedCounts;

using Blazorise.Charts;

using CollinCountyCovidDashboard.Client.Shared;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client.Pages
{
    public partial class HospitalBedsTrend
    {
        #region Dependencies

        [Inject] IGetHospitalBedCountsQuery _getHospitalBedCountsQuery { get; set; }

        #endregion

        #region Fields

        bool _isHandlingEvent;
        LineChart<int> _lineChart;
        NumDaysSlider _numDaysSlider;

        object _chartOptions = new
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
                duration = 0
            }
        };

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
            if (_isHandlingEvent) return;
            _isHandlingEvent = true;
            NumDays = _numDaysSlider.NumDays;
            await LoadChartData();
            StateHasChanged();
            _isHandlingEvent = false;
        }

        #endregion

        #region Private Methods

        private async Task LoadChartData()
        {
            var queryResults = await _getHospitalBedCountsQuery.Execute(NumDays);
            await SetChartData(queryResults.Result);
        }

        private async Task SetChartData(HospitalBedModel[] queryResults)
        {
            await _lineChart.Clear();
            await _lineChart.AddLabelsDatasetsAndUpdate(
                    GetDateLabels(queryResults),
                    GetCovidOccupiedBedsDataSet(queryResults),
                    GetOcuppiedBedsDataSet(queryResults),
                    GetTotalBedsDataSet(queryResults)
            );
        }

        private LineChartDataset<int> GetCovidOccupiedBedsDataSet(HospitalBedModel[] queryResults)
        {
            return new LineChartDataset<int>
            {
                Data = queryResults.Select(r => r.CovidOccupied).ToList(),
                BackgroundColor = new List<string> { ChartColor.FromRgba(255, 0, 0, 0.5f) },
                BorderColor = new List<string> { ChartColor.FromRgba(255, 0, 0, 1f) },
                Fill = true,
                SteppedLine = false,
                Label = "Covid Occupied Beds",
                PointRadius = 2,
                PointBorderColor = Enumerable.Repeat("#ff0000", queryResults.Length).ToList(),
                PointBackgroundColor = Enumerable.Repeat("#ff0000", queryResults.Length).ToList()
            };
        }

        private LineChartDataset<int> GetOcuppiedBedsDataSet(HospitalBedModel[] queryResults)
        {
            return new LineChartDataset<int>
            {
                Data = queryResults.Select(r => r.Occupied).ToList(),
                BackgroundColor = new List<string> { ChartColor.FromRgba(255, 255, 0, 0.5f) },
                BorderColor = new List<string> { ChartColor.FromRgba(255, 255, 0, 1f) },
                Fill = true,
                SteppedLine = false,
                Label = "Occupied Beds",
                PointRadius = 2,
                PointBorderColor = Enumerable.Repeat("#ffff00", queryResults.Length).ToList(),
                PointBackgroundColor = Enumerable.Repeat("#ffff00", queryResults.Length).ToList()
            };
        }

        private LineChartDataset<int> GetTotalBedsDataSet(HospitalBedModel[] queryResults)
        {
            return new LineChartDataset<int>
            {
                Data = queryResults.Select(r => r.Total).ToList(),
                BackgroundColor = new List<string> { ChartColor.FromRgba(0, 255, 0, 0.5f) },
                BorderColor = new List<string> { ChartColor.FromRgba(0, 255, 0, 1f) },
                Fill = true,
                SteppedLine = false,
                Label = "Total Beds",
                PointRadius = 2,
                PointBorderColor = Enumerable.Repeat("#00ff00", queryResults.Length).ToList(),
                PointBackgroundColor = Enumerable.Repeat("#00ff00", queryResults.Length).ToList()
            };
        }

        private IReadOnlyCollection<string> GetDateLabels(HospitalBedModel[] queryResults)
        {
            return queryResults.Select(d => d.Date.ToShortDateString()).ToArray();
        }



        #endregion

    }
}
