using System.Windows.Input;
using AbisMonitor.Domain;
using AbisMonitor.UI.Controls;
using AbisMonitor.UI.Models;
using AbisMonitor.Utils;
using AbisMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace AbisMonitor.UI.ViewModels
{
    public delegate void SelectModelChangedHandle(TabItemModel model);
    public class TabItemViewModel:BaseViewModel
    {
        public event SelectModelChangedHandle SelectModelChangedEvent;

        private ObservableCollection<TabItemModel> _tabItems = new ObservableCollection<TabItemModel>();
        public ObservableCollection<TabItemModel> TabItems { get { return _tabItems; } }


        public TabItemViewModel()
        {
           
        }



        private TabItemModel _selectedModel;

        public TabItemModel SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                if (_selectedModel == value) return;
                _selectedModel = value;
                this.RaisePropertyChanged(() => SelectedModel);
                if (SelectModelChangedEvent != null)
                {
                    SelectModelChangedEvent.Invoke(SelectedModel);
                }
            }
        }

        public void Add(TreeViewItem treeViewItem, string tabName, string tabImageSource, string itemNamespace,
            UserControl userControl)
        {
            var item = HasItemByNamespace(itemNamespace);
            if (item == null)
            {
                item = new TabItemModel()
                {
                    TreeViewItem = treeViewItem,
                    TabName = tabName,
                    TabImageSource = tabImageSource,
                    ItemNamespace = itemNamespace,
                    UserControl = userControl
                };
                this.TabItems.Add(item);
            }

            SelectedModel = item;
        }

        public void Add(TreeViewItem treeViewItem, string tabImageSource, string itemNamespace,
            UserControl usercontrol, AbisDeviceSimple abisDeviceSimple)
        {
            var item = HasItemByDataNum(abisDeviceSimple.DataNum);
            if (item == null)
            {
                usercontrol.DataContext = new DeviceTrackViewModel(abisDeviceSimple);
                item = new TabItemModel()
                {
                    TreeViewItem = treeViewItem,
                    TabName = string.Format("基站：{2}  （时隙{3}）", abisDeviceSimple.DeviceNameStr,
                abisDeviceSimple.PortNum, abisDeviceSimple.BtsNameStr, abisDeviceSimple.SlotNum),
                    TabImageSource = tabImageSource,
                    ItemNamespace = itemNamespace,
                    UserControl = usercontrol,
                    DataNum=abisDeviceSimple.DataNum,
                };
                this.TabItems.Add(item);
            }
            SelectedModel = item;
        }

        private TabItemModel HasItemByNamespace(string itemNamespace)
        {
            var hasItem =
                TabItems.FirstOrDefault(item => string.Equals(item.ItemNamespace, itemNamespace, StringComparison.CurrentCultureIgnoreCase));
            return hasItem;
        }

        private TabItemModel HasItemByDataNum(int dataNum)
        {
            if (dataNum < 0) return null;
            var hasItem = TabItems.FirstOrDefault(item => item.DataNum == dataNum);
            return hasItem;
        }


        #region ICommand
        public ICommand TabItemCloseCmd
        {
            get { return new RelayCommand<TabItemModel>(OnTabItemClose, AlwaysTrueHasParame); }
        }

        public ICommand CloseTabCmd
        {
            get { return new RelayCommand(OnCloseTab, AlwaysTrue); }
        }

        public ICommand CloseAllTabCmd
        {
            get { return new RelayCommand(OnCloseAllTab, AlwaysTrue); }
        }

        public ICommand CloseOtherTabCmd
        {
            get { return new RelayCommand(OnCloseOtherTab, AlwaysTrue); }
        }



        private bool AlwaysTrueHasParame(TabItemModel obj)
        {
            return true;
        }

        private void OnTabItemClose(TabItemModel model)
        {
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.ItemNamespace)) return;
            var hasItem = HasItemByNamespace(model.ItemNamespace);
            if (hasItem == null) return;

            if (TabItems.Contains(hasItem))
            {
                TabItems.Remove(hasItem);
            }
        }
        private void OnCloseTab()
        {
            TabItems.Remove(SelectedModel);
        }

        private void OnCloseAllTab()
        {
            TabItems.Clear();
        }

        private void OnCloseOtherTab()
        {
            List<TabItemModel> deleteItems = TabItems.Where(
                item =>
                    item.ItemNamespace != SelectedModel.ItemNamespace || item.DataNum!=SelectedModel.DataNum)
                .ToList();


            foreach (var item in deleteItems)
            {
                TabItems.Remove(item);
            }
        }

        #endregion

      

    }
}
