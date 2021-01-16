using System;
using System.Windows.Input;

namespace Frontend.Utility
{
    /// <summary>
    /// An <see cref="ICommand"/> implementation, providing the basic functionalities of
    /// invoking a command and, optionally, providing a check whether it can be executed.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The method that will be executed.
        /// </summary>
        readonly Action targetExecuteMethod;

        /// <summary>
        /// A function that returns a boolean, indicating if the command can be executed.
        /// </summary>
        readonly Func<bool> targetCanExecuteMethod;

        /// <summary>
        /// Creates a new instance of the class that can always be executed and calls the
        /// given "executeMethod" when done so.
        /// </summary>
        /// <param name="executeMethod">The <see cref="Action"/> to call on execution.</param>
        public RelayCommand(Action executeMethod)
        {
            targetExecuteMethod = executeMethod;
        }

        /// <summary>
        /// Creates a new instance of the class that uses the given "canExecuteMethod" to
        /// determine if the command can be executed and calls the given "executeMethod" on execution.
        /// </summary>
        /// <param name="executeMethod">The <see cref="Action"/> to call on execution.</param>
        /// <param name="canExecuteMethod">The function whose return value determines if the command can be executed</param>
        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            targetExecuteMethod = executeMethod;
            targetCanExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Raises an event that checks if the command can execute in its current state.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        // Beware - should use weak references if command instance lifetime is longer than lifetime of UI objects that get hooked up to command
        // Prism commands solve this in their implementation
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            if (targetCanExecuteMethod != null)
                return targetCanExecuteMethod();

            if (targetExecuteMethod != null)
                return true;

            return false;
        }

        public void Execute(object parameter)
        {
            targetExecuteMethod?.Invoke();
        }
    }

    /// <summary>
    /// An <see cref="ICommand"/> implementation, providing the basic functionalities of
    /// invoking a command and, optionally, providing a check whether it can be executed.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// The method that will be executed.
        /// </summary>
        Action<T> targetExecuteMethod;

        /// <summary>
        /// A function that returns a boolean, indicating if the command can be executed.
        /// </summary>
        Func<T, bool> targetCanExecuteMethod;

        /// <summary>
        /// Creates a new instance of the class that can always be executed and calls the
        /// given "executeMethod" when done so.
        /// </summary>
        /// <param name="executeMethod">The <see cref="Action{T}"/> to call on execution.</param>
        public RelayCommand(Action<T> executeMethod)
        {
            targetExecuteMethod = executeMethod;
        }

        /// <summary>
        /// Creates a new instance of the class that uses the given "canExecuteMethod" to
        /// determine if the command can be executed and calls the given "executeMethod" on execution.
        /// </summary>
        /// <param name="executeMethod">The <see cref="Action{T}"/> to call on execution.</param>
        /// <param name="canExecuteMethod">The function whose return value determines if the command can be executed</param>
        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            targetExecuteMethod = executeMethod;
            targetCanExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Raises an event that checks if the command can execute in its current state.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            if (targetCanExecuteMethod != null)
            {
                T tparam = (T)parameter;
                return targetCanExecuteMethod(tparam);
            }

            if (targetExecuteMethod != null)
                return true;

            return false;
        }

        // Beware - should use weak references if command instance lifetime is longer than lifetime of UI objects that get hooked up to command
        // Prism commands solve this in their implementation
        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter)
        {
            targetExecuteMethod?.Invoke((T)parameter);
        }
    }
}
