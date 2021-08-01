using MinesweeperML.Business.Attributes;

namespace MinesweeperML.Enumerations
{
    /// <summary>
    /// Difficulty.
    /// </summary>
    public enum Difficulty
    {
        /// <summary>
        /// The easy.
        /// </summary>
        [LocalizedDescription(nameof(Cultures.Resources.Easy))]
        Easy,

        /// <summary>
        /// The medium.
        /// </summary>
        [LocalizedDescription(nameof(Cultures.Resources.Medium))]
        Medium,

        /// <summary>
        /// The hard.
        /// </summary>
        [LocalizedDescription(nameof(Cultures.Resources.Hard))]
        Hard,

        /// <summary>
        /// The custom.
        /// </summary>
        [LocalizedDescription(nameof(Cultures.Resources.Custom))]
        Custom,
    }
}