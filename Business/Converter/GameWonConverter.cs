using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace MinesweeperML.Business.Converter
{
    /// <summary>
    /// Game won converter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class GameWonConverter : IValueConverter
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
            var gameWon = (bool?)value;
            if (gameWon != true)
            {
                return gameWon == null ? new PackIcon
                {
                    Kind = PackIconKind.EmoticonNeutralOutline,
                    Width = 20,
                    Height = 20,
                }
                : new PackIcon
                {
                    Kind = PackIconKind.EmoticonSadOutline,
                    Width = 20,
                    Height = 20,
                };
            }
            else
            {
                return new PackIcon
                {
                    Kind = PackIconKind.SmileyHappyOutline,
                    Width = 20,
                    Height = 20,
                };
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
        /// <exception cref="NotImplementedException">Should never be called.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Should never be called.");
        }
    }
}