using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MinesweeperML.Business.Interfaces;

namespace MinesweeperML.Business
{
    /// <summary>
    /// Handles errors.
    /// </summary>
    /// <seealso cref="IErrorHandler" />
    public class ErrorHandler : IErrorHandler
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>This instance.</returns>
        public static IErrorHandler Create()
        {
            return new ErrorHandler();
        }

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void HandleError(Exception ex)
        {
            var errorMessage = $"{Cultures.Resources.MessageUnexpectedError}: {ex.Message}";
            MessageBox.Show(errorMessage, Cultures.Resources.Fehler, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}