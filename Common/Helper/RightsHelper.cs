using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public static class RightsHelper
    {
        public static string RightsConvert(int rights)
        {
            switch (rights)
            {
                case 0:
                    return "操作员";
                case 1:
                    return "管理员";
                default:
                    return string.Format("未知({0})", rights);
            }
        }
    }
}
