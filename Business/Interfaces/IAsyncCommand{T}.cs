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
    /// <typeparam name="T">T.</typeparam>
    /// <seealso cref="ICommand" />
    public interface IAsyncCommand<T> : ICommand
    {
        /// <summary>
        /// Determines whether this instance can execute the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can execute the specified parameter; otherwise,
        /// <c>false</c>.
        /// </returns>
        bool CanExecute(T parameter);

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The task that is executed async.</returns>
        Task ExecuteAsync(T parameter);
    }
}