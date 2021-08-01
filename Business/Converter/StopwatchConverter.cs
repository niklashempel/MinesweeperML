using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace MinesweeperML.Business.Converter
{
    /// <summary>
    /// Stopwatch converter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class StopwatchConverter : IValueConverter
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
            var sw = (Stopwatch)value;
            if (sw == null)
            {
                return new TimeSpan(0, 0, 0).ToString();
            }
            if (sw.ElapsedMilliseconds < 10000)
            {
                return $"{sw.Elapsed}";
            }
            else if (sw.Elapsed.Hours > 99)
            {
                return $"99:59:59";
            }
            else
            {
                return $"{sw.Elapsed.Hours:D2}:{sw.Elapsed.Minutes:D2}:{sw.Elapsed.Seconds:D2}";
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