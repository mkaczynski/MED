using System;

namespace UniversalPreferences.App.ViewModels
{
    public class ViewContainer
    {
        public object View { get; private set; }
        public Action Action { get; private set; }
        public Func<bool> CanExecute { get; private set; } 

        public ViewContainer(object view, Action action, Func<bool> canExecute)
        {
            View = view;
            Action = action;
            CanExecute = canExecute;
        }
    }
}