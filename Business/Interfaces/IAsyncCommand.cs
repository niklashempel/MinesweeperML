using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MinesweeperML.Business.Interfaces
{
    /// <summary>
    /// Defines the asynchronous command.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </returns>
        bool CanExecute();

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task ExecuteAsync();
    }
}