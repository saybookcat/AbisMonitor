using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class SysParam : DbModel
    {
        public SysParam()
        {
            DbCleanExp = 90;
            FirstDay = -25;
            FirstDayTime = 64800;
            StartTime = 0;

        }

        /// <summary>
        /// 数据库清理天数
        /// </summary>
        public int DbCleanExp { get; set; }

        public int FirstDay { get; set; }

        public int FirstDayTime { get; set; }


        public int StartTime { get; set; }

    }
}
