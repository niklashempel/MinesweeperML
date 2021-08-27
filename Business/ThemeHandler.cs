using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;

namespace MinesweeperML.Business
{
    /// <summary>
    /// Handles the theme.
    /// </summary>
    public static class ThemeHandler
    {
        private static readonly PaletteHelper paletteHelper = new PaletteHelper();

        /// <summary>
        /// Changes the theme.
        /// </summary>
        /// <param name="useDarkmode">if set to <c>true</c> [use darkmode].</param>
        public static void ChangeTheme(bool useDarkmode)
        {
            var theme = paletteHelper.GetTheme();
            var baseTheme = useDarkmode ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            paletteHelper.SetTheme(theme);
            if (Properties.Settings.Default.IsDarkmode != useDarkmode)
            {
                Properties.Settings.Default.IsDarkmode = useDarkmode;
                Properties.Settings.Default.Save();
            }
        }
    }
}