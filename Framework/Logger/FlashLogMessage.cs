using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Logger
{
    public class FlashLogMessage
    {
        public object Message { get; set; }
        public FlashLogLevel Level { get; set; }

        public Exception Exception { get; set; }
    }
}
