using System;
using System.Diagnostics;
using System.Windows.Input;
using MinesweeperML.Business.Interfaces;

namespace MinesweeperML.Business.Commands
{
    /// <summary>
    /// The relay command of type T..
    /// </summary>
    /// <typeparam name="T">T.</typeparam>
    /// <seealso cref="System.Windows.Input.ICommand" />
    [DebuggerStepThrough]
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> canExecute;
        private readonly IErrorHandler errorHandler;
        private readonly Action<T> execute;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should
        /// execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RelayCommand(Action<T> execute)
                   : this(execute, null)
        {
            this.execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="ArgumentNullException">execute.</exception>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute;
            errorHandler = ErrorHandler.Create();
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its
        /// current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to <see langword="null" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if this command can be executed; otherwise, <see
        /// langword="false" />.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke((T)parameter) ?? true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to <see langword="null" />.
        /// </param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    execute((T)parameter);
                }
                catch (Exception ex)
                {
                    errorHandler.HandleError(ex);
                }
            }
        }
    }
}