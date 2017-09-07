using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AbisMonitor.Domain;
using AbisMonitor.Service.DbServices;
using AbisMonitor.UI.Messager;
using AbisMonitor.UI.Models;
using System.Threading;
using AbisMonitor.Utils;
using Framework;

namespace AbisMonitor.UI.ViewModels
{
    public class DeviceNavigationViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private readonly DeviceSimpleService _service;

        public DeviceNavigationViewModel()
        {
            _service = new DeviceSimpleService();
        }

        private Dictionary<int,Device> _deviceDic = new Dictionary<int,Device>();

        public Dictionary<int,Device> DeviceDic
        {
            get { return _deviceDic; }
            set
            {
                _deviceDic = value;
                this.RaisePropertyChanged(() => DeviceDic);
            }
        }

        #region Load

        public void LoadAsync()
        {
            GetDeviceListAsync();
        }

        private void GetDeviceListAsync()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    List<AbisDeviceSimple> list = _service.GetAbisDeviceSimpleList();
                    if (list == null) return;
                    Dictionary<int,Device> deviceList = new Dictionary<int,Device>();
                    foreach (AbisDeviceSimple item in list)
                    {
                        //Add Device
                        if (!deviceList.Keys.Contains(item.DeviceNum))
                        {
                            var device = new Device()
                            {
                                DeviceName = item.DeviceNameStr,
                                DeviceNum = item.DeviceNum,
                            };
                            deviceList.Add(item.DeviceNum, device);
                        }

                        //Add port
                     
                        if (deviceList[item.DeviceNum].PortDic == null)
                        {
                            deviceList[item.DeviceNum].PortDic = new Dictionary<int, Port>();
                        }
                        if (!deviceList[item.DeviceNum].PortDic.Keys.Contains(item.PortNum))
                        {
                            var port = new Port()
                            {
                                PortNum = item.PortNum
                            };
                            deviceList[item.DeviceNum].PortDic.Add(item.PortNum, port);
                        }

                        //Add cell
                        if (deviceList[item.DeviceNum].PortDic[item.PortNum].CellDic == null)
                        {
                            deviceList[item.DeviceNum].PortDic[item.PortNum].CellDic = new Dictionary<int, Cell>();
                        }
                        if (!deviceList[item.DeviceNum].PortDic[item.PortNum].CellDic.Keys.Contains(item.DataNum))
                        {
                            var cell = new Cell()
                            {
                                DataNum = item.DataNum,
                                BtsName = item.BtsNameStr,
                                IpAddress = item.IpAddress,
                                SlotNum = item.SlotNum,
                                AbisDeviceSimple = item,
                            };
                            deviceList[item.DeviceNum].PortDic[item.PortNum].CellDic.Add(item.DataNum, cell);
                        }
                    }
                    DeviceDic = deviceList;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            });
        }
        #endregion 

        #region OnDeviceTrackCmd

        public ICommand OnDeviceTrackCmd
        {
            get { return new RelayCommand<AbisDeviceSimple>(OnDeviceTrack, AlwaysTrue); }
        }

        private bool AlwaysTrue(AbisDeviceSimple obj)
        {
            return true;
        }

        private void OnDeviceTrack(AbisDeviceSimple abisDeviceSimple)
        {
            if (abisDeviceSimple == null) return;
            AbisDeviceSimpleSelectedItem = abisDeviceSimple;
        }

        #endregion

        private AbisDeviceSimple _abisDeviceSimpleSelectedItem;

        public AbisDeviceSimple AbisDeviceSimpleSelectedItem
        {
            get { return _abisDeviceSimpleSelectedItem; }
            set
            {
                if (_abisDeviceSimpleSelectedItem == value) return;
                _abisDeviceSimpleSelectedItem = value;
                this.RaisePropertyChanged(() => AbisDeviceSimpleSelectedItem);
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new DeviceTrackModelChangedMessage()
                {
                    AbisDeviceSimple = AbisDeviceSimpleSelectedItem
                });
            }
        }
    }
}
