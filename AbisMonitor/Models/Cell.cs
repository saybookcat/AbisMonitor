using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbisMonitor.Domain;
using AbisMonitor.Service.DbServices;

namespace AbisMonitor.UI.Models
{
    public class Cell : GalaSoft.MvvmLight.ObservableObject
    {
        public int DataNum { get; set; }

        public string BtsName { get; set; }

        public int SlotNum { get; set; }

        public string IpAddress { get; set; }

        public AbisDeviceSimple AbisDeviceSimple { get; set; }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                this.RaisePropertyChanged(() => IsSelected);
            }
        }


    }
}
