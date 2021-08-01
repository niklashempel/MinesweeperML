using System;
using System.Threading.Tasks;
using MinesweeperML.Business.Interfaces;

namespace MinesweeperML.Business.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Task"></see>.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Fires the and forget safety.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="handler">The handler.</param>
        public static async void FireAndForgetSafety(this Task task, IErrorHandler handler)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler.HandleError(ex);
            }
        }
    }
}