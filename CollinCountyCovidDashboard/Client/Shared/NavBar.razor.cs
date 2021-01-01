using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace CollinCountyCovidDashboard.Client.Shared
{

    public enum NavPageType
    {
        Latest,
        SevenDayAvg,
        NewCaseTrends,
        NewDeathsTrend,
        PositivityRateTrend,
        HospitalBedsTrend,
        Sources
    }


    public partial class NavBar
    {
        [Inject] public NavigationManager _navigationManager { get; set; }

        public NavPageType CurrentPage => GetPageTypeForCurrentUrl();

        public string CurrentPageName => GetCurrentPageName();

        public void GoToNextPage()
        {
            _navigationManager.NavigateTo(GetNextPage().ToString());
            //StateHasChanged();
        }

        public void GoToPreviousPage()
        {
            _navigationManager.NavigateTo(GetPrevPage().ToString());
            //StateHasChanged();
        }


        private NavPageType GetPageTypeForCurrentUrl()
        {
            var curUrl = _navigationManager.Uri;
            var finalRoute = curUrl.Split(@"/").Last();
            var returnPage = NavPageType.Latest;
            Enum.TryParse(finalRoute, out returnPage);
            return returnPage;
        }

        private string GetCurrentPageName()
        {
            var curPage = CurrentPage;
            switch (curPage)
            {
                case NavPageType.SevenDayAvg:
                    return "7 Day Averages";
                case NavPageType.NewCaseTrends:
                    return "New Cases Trend";
                case NavPageType.NewDeathsTrend:
                    return "New Deaths Trend";
                case NavPageType.PositivityRateTrend:
                    return "Positivity Rate Trend";
                case NavPageType.HospitalBedsTrend:
                    return "Hospital Beds Trend";
                default:
                    return curPage.ToString();
            }
        }

        private NavPageType GetNextPage()
        {
            var pageTypes = Enum.GetValues(typeof(NavPageType)).Cast<NavPageType>();
            var curPageIndex = (int)CurrentPage;
            var nextPageIndex = curPageIndex + 1;
            if (nextPageIndex >= pageTypes.Count()) nextPageIndex = 0;
            return (NavPageType)nextPageIndex;
        }

        private NavPageType GetPrevPage()
        {
            var pageTypes = Enum.GetValues(typeof(NavPageType)).Cast<NavPageType>();
            var curPageIndex = (int)CurrentPage;
            var prevPageIndex = curPageIndex -1;
            if (prevPageIndex < 0) prevPageIndex = pageTypes.Count()-1;
            return (NavPageType)prevPageIndex;
        }

    }
}
