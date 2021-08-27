using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperML.Enumerations
{
    /// <summary>
    /// The feature enumeration.
    /// </summary>
    public enum Feature
    {
        /// <summary>
        /// The out of field
        /// </summary>
        OutOfField = -2,

        /// <summary>
        /// The unknown
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// The no bomb
        /// </summary>
        NoBomb = 0,

        /// <summary>
        /// The one bomb
        /// </summary>
        OneBomb,

        /// <summary>
        /// The two bombs
        /// </summary>
        TwoBombs,

        /// <summary>
        /// The three bombs
        /// </summary>
        ThreeBombs,

        /// <summary>
        /// The four bombs
        /// </summary>
        FourBombs,

        /// <summary>
        /// The five bombs
        /// </summary>
        FiveBombs,

        /// <summary>
        /// The six bombs
        /// </summary>
        SixBombs,

        /// <summary>
        /// The seven bombs
        /// </summary>
        SevenBombs,

        /// <summary>
        /// The eight bombs
        /// </summary>
        EightBombs,

        /// <summary>
        /// The marked
        /// </summary>
        Marked = 9,
    }
}