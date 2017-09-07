using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbisMonitor.UI.ClientSocket
{
    public interface IParseEngine
    {
        void Start();
        void Stop();

        void Pause();

        void Resume();

        void Append(byte[] buffer);

        int Count { get; }
    }
}
