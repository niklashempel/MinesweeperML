namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// Game won dialog view model.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    public class GameWonDialogViewModel : BaseViewModel
    {
        private string winningTime;

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public string WinningTime
        {
            get
            {
                return winningTime;
            }
            set
            {
                if (value != winningTime)
                {
                    winningTime = value;
                    NotifyPropertyChanged(nameof(WinningTime));
                }
            }
        }
    }
}