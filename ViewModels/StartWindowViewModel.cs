using MinesweeperML.Business.Database.DbContexts;

namespace MinesweeperML.ViewsModel
{
    /// <summary>
    /// Start window view model.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    public class StartWindowViewModel : BaseViewModel
    {
        private BaseViewModel selectedViewModel;

        /// <summary>
        /// Gets or sets the selected view model.
        /// </summary>
        /// <value>The selected view model.</value>
        public BaseViewModel SelectedViewModel
        {
            get
            {
                return selectedViewModel;
            }
            set
            {
                if (value != selectedViewModel)
                {
                    selectedViewModel = value;
                    NotifyPropertyChanged(nameof(SelectedViewModel));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartWindowViewModel" /> class.
        /// </summary>
        /// <param name="mainMenuViewModel">The main menu view model.</param>
        public StartWindowViewModel(MainMenuViewModel mainMenuViewModel)
        {
            SelectedViewModel = mainMenuViewModel;
            mainMenuViewModel.StartWindowViewModel = this;
        }
    }
}