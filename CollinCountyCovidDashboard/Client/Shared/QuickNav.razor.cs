using Microsoft.AspNetCore.Components.Web;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client.Shared
{
    public partial class QuickNav
    {
        #region Properties

        private string MenuAnimationClass { get; set; }

        #endregion

        #region Event Handler

        private void MenuButtonClicked(MouseEventArgs e)
        {
            ShowMenu();
        }

        private void ClickAwayRegionClicked(MouseEventArgs e)
        {
            HideMenu();
        }

        private void NavLinkClicked(MouseEventArgs e)
        {
            HideMenu();
        }

        #endregion

        #region Private Methods

        private void ShowMenu()
        {
            MenuAnimationClass = "slideOut";
        }

        private void HideMenu()
        {
            MenuAnimationClass = "slideIn";
        }

        #endregion
    }
}
