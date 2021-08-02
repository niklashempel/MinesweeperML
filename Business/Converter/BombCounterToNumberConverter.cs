using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using MinesweeperML.Models;
using MinesweeperML.ViewModels;

namespace MinesweeperML.Business.Converter
{
    /// <summary>
    /// Bomb counter to number converter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class BombCounterToNumberConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <see langword="null" />, the valid
        /// null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tile = (TileViewModel)value ?? new TileViewModel();
            if (tile.IsBomb)
            {
                return new PackIcon { Kind = PackIconKind.Bomb };
            }
            else
            {
                return tile.SurroundingBombs.ToString();
            }
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <see langword="null" />, the valid
        /// null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}