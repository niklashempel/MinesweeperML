using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MinesweeperML.Business.Extensions;
using MinesweeperML.Business.Interfaces;

namespace MinesweeperML.Business.Commands
{
    /// <summary>
    /// Implements asynchronous command.
    /// </summary>
    /// <seealso cref="IAsyncCommand" />
    [DebuggerStepThrough]
    public class AsyncCommand : IAsyncCommand
    {
        private readonly Func<bool> canExecute;

        private readonly IErrorHandler errorHandler;

        private readonly Func<Task> execute;

        private bool isExecuting;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should
        /// execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public AsyncCommand(
            Func<Task> execute,
            Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.errorHandler = ErrorHandler.Create();
        }

        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExecute()
        {
            return !isExecuting && (canExecute?.Invoke() ?? true);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync().FireAndForgetSafety(errorHandler);
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    isExecuting = true;
                    await execute();
                }
                finally
                {
                    isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}