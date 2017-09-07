using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Windows.Controls;
using System.Windows;
using AbisMonitor.ViewModels;

namespace AbisMonitor.UI.Models
{
    public class TabItemModel: GalaSoft.MvvmLight.ObservableObject
    {
        private int _dataNum = -1;

        public int DataNum
        {
            get { return _dataNum; }
            set
            {
                _dataNum = value;
                this.RaisePropertyChanged(() => DataNum);
            }
        }

        public TreeViewItem TreeViewItem { get; set; }

        private string _itemNamespace;

        public string ItemNamespace
        {
            get { return _itemNamespace; }
            set
            {
                _itemNamespace = value;
                this.RaisePropertyChanged(() => ItemNamespace);
            }
        }

        private string _tabName;
        public string TabName
        {
            get { return _tabName; }
            set
            {
                if (_tabName == value) return;
                _tabName = value;
                RaisePropertyChanged(() => TabName);
            }
        }

        private string _tabImageSource;

        public string TabImageSource
        {
            get { return _tabImageSource; }
            set
            {
                _tabImageSource = value;
                this.RaisePropertyChanged(() => TabImageSource);
            }
        }

        private UserControl _userControl;
        public UserControl UserControl
        {
            get
            {
                return _userControl;
            }
            set
            {
                _userControl = value;
                this.RaisePropertyChanged(() => UserControl);
            }
        }

    }
}
