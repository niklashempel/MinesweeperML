using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperML.Business.Interfaces
{
    /// <summary>
    /// Defines the ErrorHandler.
    /// </summary>
    public interface IErrorHandler
    {
        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="ex">The exception.</param>
        void HandleError(Exception ex);
    }
}