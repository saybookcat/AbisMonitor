using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class User : DbModel
    {
        public string LoginName { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// 0:操作员
        /// 1:管理员
        /// </summary>
        public int Rights { get; set; }

        public string RealName { get; set; }
    }
}
