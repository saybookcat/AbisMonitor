using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace AbisMonitor.UI.ViewModels
{
    /// <summary>
    /// Implements INotifyPropertyChanged for all ViewModel
    /// </summary>
    public abstract class BaseViewModel : GalaSoft.MvvmLight.ViewModelBase
    {

        protected bool AlwaysTrue() { return true; }


        #region CanCelQuery
        protected CancellationTokenSource CancelQueryTokenSource { get; set; }

        public ICommand CancelQueryCmd {
            get { return new RelayCommand(OnCancelQuery, AlwaysTrue); }
        }

        private void OnCancelQuery()
        {
            if (CancelQueryTokenSource != null)
            {
                CancelQueryTokenSource.Cancel();
            }
        }

        private Visibility _isLoading = Visibility.Collapsed;

        public Visibility IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                this.RaisePropertyChanged(() => IsLoading);
            }
        }

        protected void ShowLoading()
        {
            IsLoading = Visibility.Visible;
        }

        protected void HideLoading()
        {
            IsLoading = Visibility.Collapsed;
        }

        #endregion

    }
}
