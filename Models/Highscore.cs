namespace MinesweeperML.Models
{
    /// <summary>
    /// Highscore.
    /// </summary>
    public class Highscore
    {
        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        /// <value>The difficulty.</value>
        public int Difficulty { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public long Time { get; set; }
    }
}