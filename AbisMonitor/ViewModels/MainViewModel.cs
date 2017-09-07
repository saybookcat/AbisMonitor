using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using AbisMonitor.Domain;
using AbisMonitor.UI.Messager;
using AbisMonitor.Utils;
using AbisMonitor.ViewModels;
using AbisMonitor.Views;
using Framework;
using MvvmDialogs;

namespace AbisMonitor.UI.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #region Parameters
        private readonly IDialogService DialogService;

        /// <summary>
        /// Title of the application, as displayed in the top bar of the window
        /// </summary>
        public string Title
        {
            get { return "AbisMonitor"; }
        }

        public TabItemViewModel TabItemViewModel { get; private set; }

        public DeviceNavigationViewModel DeviceNavigationViewModel { get; set; }

        #endregion

        #region Constructors
        public MainViewModel()
        {
            // DialogService is used to handle dialogs
            this.DialogService = new MvvmDialogs.DialogService();
            TabItemViewModel = new TabItemViewModel();
            TabItemViewModel.SelectModelChangedEvent += TabItemViewModel_SelectModelChangedEvent;

            DeviceNavigationViewModel = new DeviceNavigationViewModel();
            DeviceNavigationViewModel.LoadAsync();
        }

        #endregion

        #region Methods

        #endregion

        #region Commands

        public ICommand ModifyUserInfoCmd
        {
            get { return new RelayCommand(OnModifyUserInfo, AlwaysTrue); }
        }

        public ICommand ModifyPwdCmd
        {
            get { return new RelayCommand(OnModifyPwd, AlwaysTrue); }
        }

        public ICommand LogoutCmd
        {
            get { return new RelayCommand(OnLogout, AlwaysTrue); }
        }

        public ICommand ShowAboutDialogCmd { get { return new RelayCommand(OnShowAboutDialog, AlwaysTrue); } }
        public ICommand ExitCmd { get { return new RelayCommand(OnExitApp, AlwaysTrue); } }

        private void OnModifyPwd()
        {
        }

        private void OnModifyUserInfo()
        {
        }

        private void OnShowAboutDialog()
        {
            Log.Info("Opening About dialog");
            AboutViewModel dialog = new AboutViewModel();
            var result = DialogService.ShowDialog<About>(this, dialog);
        }
        private void OnExitApp()
        {
            System.Windows.Application.Current.MainWindow.Close();
        }

        private void OnLogout()
        {
        }


        #endregion


        #region public methods

        public void AddTabItemControl(TreeViewItem treeViewItem,string headerTitle,string headerImageSource, string itemNamespace, UserControl tabControlItem)
        {
            TabItemViewModel.Add(treeViewItem, headerTitle, headerImageSource, itemNamespace, tabControlItem);
        }

        private void TabItemViewModel_SelectModelChangedEvent(UI.Models.TabItemModel model)
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<UserControlSelectChangedMessage>(
                new UserControlSelectChangedMessage() {TabItemModel = model});
        }

        public void AddTabDeviceTrack(TreeViewItem treeViewItem, string headerImageSource, string itemNamespace,
            UserControl tabControlItem,AbisDeviceSimple abisDeviceSimple)
        {
            if (abisDeviceSimple == null) return;
            TabItemViewModel.Add(treeViewItem, headerImageSource, itemNamespace, tabControlItem, abisDeviceSimple);
        }


        public string Version
        {
            get
            {
                Version ver = Assembly.GetExecutingAssembly().GetName().Version;
                string strVer = string.Format("V {0}.{1} (B{2:D02}{3:D02}{4:D02})", ver.Major, 0, ver.Minor, ver.Build,
                    ver.MinorRevision);

                strVer = strVer.Replace(" ", "");
                return strVer;
            }
        }
        #endregion 


        #region Events

        #endregion
    }
}
