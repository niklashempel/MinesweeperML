using System;
using System.Diagnostics;
using System.Windows.Input;
using MinesweeperML.Business.Interfaces;

namespace MinesweeperML.Business.Commands
{
    /// <summary>
    /// Relay command.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    [DebuggerStepThrough]
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> canExecute;

        private readonly IErrorHandler errorHandler;

        private readonly Action<object> execute;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should
        /// execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="NullReferenceException">execute.</exception>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute ?? throw new NullReferenceException(nameof(execute));
            this.canExecute = canExecute;
            errorHandler = ErrorHandler.Create();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
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
            return canExecute?.Invoke(parameter) ?? true;
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
                    execute(parameter);
                }
                catch (Exception ex)
                {
                    errorHandler.HandleError(ex);
                }
            }
        }
    }
}