using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Configs
{
    public class LoginConfig
    {
        public string UserId { get; set; }

        public string PassWord { get; set; }

        public bool RememberPassword { get; set; }

        public bool AutoLogin { get; set; }
    }
}
