using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbisMonitor.UI.ViewModels.Monitor
{
    interface IMonitorManager
    {
        void Start();
        void Pause();
        void Stop();

    }
}
