using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// The view model representing a tile on the game board.
    /// </summary>
    /// <seealso cref="MinesweeperML.ViewModels.BaseViewModel" />
    public class TileViewModel : BaseViewModel
    {
        private bool isBomb;
        private bool isClickable;
        private bool isMarked;
        private int? surroundingBombs;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is bomb.
        /// </summary>
        /// <value><c>true</c> if this instance is bomb; otherwise, <c>false</c>.</value>
        public bool IsBomb
        {
            get
            {
                return isBomb;
            }
            set
            {
                if (value != isBomb)
                {
                    isBomb = value;
                    NotifyPropertyChanged(nameof(IsBomb));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsClickable
        {
            get
            {
                return isClickable;
            }
            set
            {
                if (value != isClickable)
                {
                    isClickable = value;
                    NotifyPropertyChanged(nameof(IsClickable));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is marked.
        /// </summary>
        /// <value><c>true</c> if this instance is marked; otherwise, <c>false</c>.</value>
        public bool IsMarked
        {
            get
            {
                return isMarked;
            }
            set
            {
                if (value != isMarked)
                {
                    isMarked = value;
                    NotifyPropertyChanged(nameof(IsMarked));
                }
            }
        }

        /// <summary>
        /// Gets or sets the surrounding bombs.
        /// </summary>
        /// <value>The surrounding bombs.</value>
        public int? SurroundingBombs
        {
            get
            {
                return surroundingBombs;
            }
            set
            {
                if (value != surroundingBombs)
                {
                    surroundingBombs = value;
                    NotifyPropertyChanged(nameof(SurroundingBombs));
                }
            }
        }
    }
}