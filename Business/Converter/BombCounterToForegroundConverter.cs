using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using MinesweeperML.Models;
using MinesweeperML.ViewsModel;

namespace MinesweeperML.Business.Converter
{
    internal class BombCounterToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tile = (TileViewModel)value ?? new TileViewModel();
            if (!tile.IsBomb)
            {
                var number = tile.SurroundingBombs;
                return number switch
                {
                    0 => new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    1 => new SolidColorBrush(Color.FromRgb(34, 115, 245)), // Blue
                    2 => new SolidColorBrush(Color.FromRgb(1, 120, 51)), // Green
                    3 => new SolidColorBrush(Color.FromRgb(217, 2, 2)), // Red
                    4 => new SolidColorBrush(Color.FromRgb(18, 0, 110)), // Dark blue
                    5 => new SolidColorBrush(Color.FromRgb(71, 33, 0)), // Brown
                    6 => new SolidColorBrush(Color.FromRgb(235, 118, 16)), // Orange
                    7 => new SolidColorBrush(Color.FromRgb(116, 16, 235)), // Purple
                    8 => new SolidColorBrush(Color.FromRgb(0, 0, 0)), // Black
                    _ => throw new NotImplementedException(nameof(number)),
                };
            }
            else
            {
                return new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}