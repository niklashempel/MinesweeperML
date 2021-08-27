using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using MinesweeperML.Business;
using MinesweeperML.Business.Commands;
using MinesweeperML.Views;

namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// ViewModel that shall be used as <see cref="SettingsMenu" />'s DataContext.
    /// </summary>
    /// <seealso cref="MinesweeperML.ViewModels.BaseViewModel" />
    public class SettingsMenuViewModel : BaseViewModel
    {
        private RelayCommand goBackCommand;
        private bool isDarkmodeEnabled;
        private MainMenuViewModel mainMenuViewModel;
        private StartWindowViewModel startWindowViewModel;

        /// <summary>
        /// Gets the go back command.
        /// </summary>
        /// <value>The go back command.</value>
        public RelayCommand GoBackCommand
        {
            get
            {
                return goBackCommand ??= new RelayCommand(param => this.GoBack());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is darkmode enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is darkmode enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDarkmodeEnabled
        {
            get => isDarkmodeEnabled;
            set
            {
                if (value != isDarkmodeEnabled)
                {
                    isDarkmodeEnabled = value;
                    NotifyPropertyChanged(nameof(isDarkmodeEnabled));
                    ToggleDarkmode(isDarkmodeEnabled);
                }
            }
        }

        /// <summary>
        /// Initializes the settings.
        /// </summary>
        public void InitializeSettings()
        {
            this.IsDarkmodeEnabled = Properties.Settings.Default.IsDarkmode;
        }

        /// <summary>
        /// Sets the main menu view model.
        /// </summary>
        /// <param name="mainMenuViewModel">The main menu view model.</param>
        public void SetMainMenuViewModel(MainMenuViewModel mainMenuViewModel)
        {
            this.mainMenuViewModel = mainMenuViewModel;
        }

        /// <summary>
        /// Sets the start window view model.
        /// </summary>
        /// <param name="startWindowViewModel">The start window view model.</param>
        public void SetStartWindowViewModel(StartWindowViewModel startWindowViewModel)
        {
            this.startWindowViewModel = startWindowViewModel;
        }

        private static void ToggleDarkmode(bool isDarkmodeEnabled)
        {
            ThemeHandler.ChangeTheme(isDarkmodeEnabled);
        }

        private void GoBack()
        {
            this.startWindowViewModel.SelectedViewModel = mainMenuViewModel;
        }
    }
}