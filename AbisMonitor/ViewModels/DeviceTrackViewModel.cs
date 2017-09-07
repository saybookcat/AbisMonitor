using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AbisMonitor.Domain;
using AbisMonitor.UI.Messager;

namespace AbisMonitor.UI.ViewModels
{
    public class DeviceTrackViewModel:QueryViewModelCommandable<AbisDeviceSimple,BaseParamViewModel>
    {
        public DeviceTrackViewModel(AbisDeviceSimple abisDeviceSimple) : base(null,null)
        {
            Title = string.Format("设备：{0}  环：{1}  基站：{2}  （时隙{3}）", abisDeviceSimple.DeviceNameStr,
                abisDeviceSimple.PortNum, abisDeviceSimple.BtsNameStr, abisDeviceSimple.SlotNum);
        }


        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.RaisePropertyChanged(() => Title);
            }
        }



    }
}
