using System;
using MinesweeperML.Enumerations;

namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// The highscore view model.
    /// </summary>
    /// <seealso cref="MinesweeperML.ViewModels.BaseViewModel" />
    public class HighscoreViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        /// <value>The difficulty.</value>
        public Difficulty Difficulty { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>The rank.</value>
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public TimeSpan Time { get; set; }
    }
}