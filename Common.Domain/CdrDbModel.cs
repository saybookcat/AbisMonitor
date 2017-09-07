using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public abstract class CdrDbModel:DbModel
    {
        public long CdrIndex { get; set; }

        public virtual DateTime CdrTime { get; set; }

        public virtual int CdrTimeUsec { get; set; }
    }
}
