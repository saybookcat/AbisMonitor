using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbisMonitor.Domain;
using Common.Service;

namespace AbisMonitor.UI.ViewModels
{
    public class AllSignalViewModel : QueryViewModelCommandable<AbisDeviceSimple, BaseParamViewModel>
    {
        public AllSignalViewModel() : base(null, null)
        {
        }
    }
}
