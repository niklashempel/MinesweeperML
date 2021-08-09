using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using MinesweeperML.Enumerations;

namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// The training data view model contains a single training view model.
    /// </summary>
    /// <seealso cref="MinesweeperML.ViewModels.BaseViewModel" />
    public class TrainingDataViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TrainingDataViewModel"
        /// /> is label.
        /// </summary>
        /// <value><c>true</c> if label; otherwise, <c>false</c>.</value>
        public bool Label { get; set; }

        /// <summary>
        /// Gets or sets the X00.
        /// </summary>
        /// <value>The X00.</value>
        [LoadColumn(1)]
        public Feature X00 { get; set; }

        /// <summary>
        /// Gets or sets the X01.
        /// </summary>
        /// <value>The X01.</value>
        [LoadColumn(2)]
        public Feature X01 { get; set; }

        /// <summary>
        /// Gets or sets the X02.
        /// </summary>
        /// <value>The X02.</value>
        [LoadColumn(3)]
        public Feature X02 { get; set; }

        /// <summary>
        /// Gets or sets the X03.
        /// </summary>
        /// <value>The X03.</value>
        [LoadColumn(4)]
        public Feature X03 { get; set; }

        /// <summary>
        /// Gets or sets the X04.
        /// </summary>
        /// <value>The X04.</value>
        [LoadColumn(5)]
        public Feature X04 { get; set; }

        /// <summary>
        /// Gets or sets the X10.
        /// </summary>
        /// <value>The X10.</value>
        [LoadColumn(6)]
        public Feature X10 { get; set; }

        /// <summary>
        /// Gets or sets the X11.
        /// </summary>
        /// <value>The X11.</value>
        [LoadColumn(7)]
        public Feature X11 { get; set; }

        /// <summary>
        /// Gets or sets the X12.
        /// </summary>
        /// <value>The X12.</value>
        [LoadColumn(8)]
        public Feature X12 { get; set; }

        /// <summary>
        /// Gets or sets the X13.
        /// </summary>
        /// <value>The X13.</value>
        [LoadColumn(9)]
        public Feature X13 { get; set; }

        /// <summary>
        /// Gets or sets the X14.
        /// </summary>
        /// <value>The X14.</value>
        [LoadColumn(10)]
        public Feature X14 { get; set; }

        /// <summary>
        /// Gets or sets the X20.
        /// </summary>
        /// <value>The X20.</value>
        [LoadColumn(11)]
        public Feature X20 { get; set; }

        /// <summary>
        /// Gets or sets the X21.
        /// </summary>
        /// <value>The X21.</value>
        [LoadColumn(12)]
        public Feature X21 { get; set; }

        /// <summary>
        /// Gets or sets the X23.
        /// </summary>
        /// <value>The X23.</value>
        [LoadColumn(13)]
        public Feature X23 { get; set; }

        /// <summary>
        /// Gets or sets the X24.
        /// </summary>
        /// <value>The X24.</value>
        [LoadColumn(14)]
        public Feature X24 { get; set; }

        /// <summary>
        /// Gets or sets the X30.
        /// </summary>
        /// <value>The X30.</value>
        [LoadColumn(15)]
        public Feature X30 { get; set; }

        /// <summary>
        /// Gets or sets the X31.
        /// </summary>
        /// <value>The X31.</value>
        [LoadColumn(16)]
        public Feature X31 { get; set; }

        /// <summary>
        /// Gets or sets the X32.
        /// </summary>
        /// <value>The X32.</value>
        [LoadColumn(17)]
        public Feature X32 { get; set; }

        /// <summary>
        /// Gets or sets the X33.
        /// </summary>
        /// <value>The X33.</value>
        [LoadColumn(18)]
        public Feature X33 { get; set; }

        /// <summary>
        /// Gets or sets the X34.
        /// </summary>
        /// <value>The X34.</value>
        [LoadColumn(19)]
        public Feature X34 { get; set; }

        /// <summary>
        /// Gets or sets the X40.
        /// </summary>
        /// <value>The X40.</value>
        [LoadColumn(20)]
        public Feature X40 { get; set; }

        /// <summary>
        /// Gets or sets the X41.
        /// </summary>
        /// <value>The X41.</value>
        [LoadColumn(21)]
        public Feature X41 { get; set; }

        /// <summary>
        /// Gets or sets the X42.
        /// </summary>
        /// <value>The X42.</value>
        [LoadColumn(22)]
        public Feature X42 { get; set; }

        /// <summary>
        /// Gets or sets the X43.
        /// </summary>
        /// <value>The X43.</value>
        [LoadColumn(23)]
        public Feature X43 { get; set; }

        /// <summary>
        /// Gets or sets the X44.
        /// </summary>
        /// <value>The X44.</value>
        [LoadColumn(24)]
        public Feature X44 { get; set; }
    }
}