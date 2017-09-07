using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DataExtension;

namespace Common.Domain
{
    public abstract class DbModel
    {
        public int Disp { get; set; }

        [DbField("DataNum")]
        public int DataNum { get; set; }
    }
}
