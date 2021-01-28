using Application.Queries.GetICUBedCounts;
using Application.Queries.GetVaccineTrendData;

using Blazorise.Charts;

using CollinCountyCovidDashboard.Client.Shared;

using Microsoft.AspNetCore.Components;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client.Pages
{
    public partial class VaccineTrend
    {
        #region Dependencies

        [Inject] IGetVaccineTrendDataQuery _getvaccineTrendDataQuery { get; set; }

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
                            fontColor = "#fff",
                            min = 0
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
            var queryResults = await _getvaccineTrendDataQuery.Execute(NumDays);
            await SetChartData(queryResults.Result);
        }

        private async Task SetChartData(VaccineTrendDataModel[] queryResults)
        {
            await _lineChart.Clear();
            await _lineChart.AddLabelsDatasetsAndUpdate(
                    GetDateLabels(queryResults),
                    GetFullyVaccinatedDataSet(queryResults),
                    GetPartiallyVaccinatedDataSet(queryResults),
                    GetAllocatedVaccineDataSet(queryResults),
                    GetEligiblePopulationDataSet(queryResults)
            );
        }

        private LineChartDataset<int> GetFullyVaccinatedDataSet(VaccineTrendDataModel[] queryResults)
        {
            return new LineChartDataset<int>
            {
                Data = queryResults.Select(r => r.PeopleGivenFullDoes).ToList(),
                BackgroundColor = new List<string> { ChartColor.FromRgba(0, 255, 0, 0.5f) },
                BorderColor = new List<string> { ChartColor.FromRgba(0, 255, 0, 1f) },
                Fill = true,
                SteppedLine = false,
                Label = "People Fully Vaccinated",
                PointRadius = 2,
                PointBorderColor = Enumerable.Repeat("#00ff00", queryResults.Length).ToList(),
                PointBackgroundColor = Enumerable.Repeat("#00ff00", queryResults.Length).ToList()
            };
        }

        private LineChartDataset<int> GetPartiallyVaccinatedDataSet(VaccineTrendDataModel[] queryResults)
        {
            return new LineChartDataset<int>
            {
                Data = queryResults.Select(r => r.PeopleGiven1Does).ToList(),
                BackgroundColor = new List<string> { ChartColor.FromRgba(255, 255, 0, 0.5f) },
                BorderColor = new List<string> { ChartColor.FromRgba(255, 255, 0, 1f) },
                Fill = true,
                SteppedLine = false,
                Label = "People Given At Least 1 Dose",
                PointRadius = 2,
                PointBorderColor = Enumerable.Repeat("#ffff00", queryResults.Length).ToList(),
                PointBackgroundColor = Enumerable.Repeat("#ffff00", queryResults.Length).ToList()
            };
        }

        private LineChartDataset<int> GetAllocatedVaccineDataSet(VaccineTrendDataModel[] queryResults)
        {
            return new LineChartDataset<int>
            {
                Data = queryResults.Select(r => r.TotalAllocated).ToList(),
                BackgroundColor = new List<string> { ChartColor.FromRgba(255, 165, 0, 0.5f) },
                BorderColor = new List<string> { ChartColor.FromRgba(255, 165, 0, 1f) },
                Fill = true,
                SteppedLine = true,
                Label = "Allocated",
                PointRadius = 2,
                PointBorderColor = Enumerable.Repeat("#ffa500", queryResults.Length).ToList(),
                PointBackgroundColor = Enumerable.Repeat("#ffa500", queryResults.Length).ToList()
            };
        }

        private LineChartDataset<int> GetEligiblePopulationDataSet(VaccineTrendDataModel[] queryResults)
        {
            return new LineChartDataset<int>
            {
                Data = queryResults.Select(r => r.EligiblePopulation).ToList(),
                BackgroundColor = new List<string> { ChartColor.FromRgba(255, 0, 0, 0.5f) },
                BorderColor = new List<string> { ChartColor.FromRgba(255, 0, 0, 1f) },
                Fill = true,
                SteppedLine = true,
                Label = "Eligible Population",
                PointRadius = 2,
                PointBorderColor = Enumerable.Repeat("#ff0000", queryResults.Length).ToList(),
                PointBackgroundColor = Enumerable.Repeat("#ff0000", queryResults.Length).ToList()
            };
        }


        private IReadOnlyCollection<string> GetDateLabels(VaccineTrendDataModel[] queryResults)
        {
            return queryResults.Select(d => d.Date.ToShortDateString()).ToArray();
        }

        #endregion

    }
}
