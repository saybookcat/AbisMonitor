using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AbisMonitor.UI.Utils
{
    public static class CommandExtension
    {
        public static ICommand Relay(this ICommand command, Action action)
        {
            if (command == null)
                command = new GalaSoft.MvvmLight.Command.RelayCommand(action);
            return command;
        }

        public static ICommand Relay<T>(this ICommand command, Action<T> action)
        {
            if (command == null)
            {
                command = new GalaSoft.MvvmLight.Command.RelayCommand<T>(action);
            }
            return command;
        }
    }
}
