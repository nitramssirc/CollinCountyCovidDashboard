using Blazorise;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client.Shared
{
    public partial class NumDaysSlider
    {
        #region Parameters

        [Parameter]
        public EventCallback NumDaysChanged { get; set; }

        #endregion

        #region View References

        Slider<int> _slider;

        #endregion

        #region Properties

        public int NumDays
        {
            get
            {
                return -_slider.Value;
            }
        }

        private int MaxNumDays
        {
            get
            {
                return (DateTime.Now - new DateTime(2020, 3, 4)).Days;
            }
        }

        #endregion

        #region EventHandlers

        private async Task OnValueChanged(int newValue)
        {
            await NumDaysChanged.InvokeAsync();
        }

        #endregion
    }
}
