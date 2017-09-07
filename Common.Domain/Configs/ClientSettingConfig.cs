using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Configs
{
    public class ClientSettingConfig
    {
        public int ExcelMaxCountForInfo { get; set; }

        public int ExcelSheetMaxCount { get; set; }

        public bool CheckBoxIsChecked { get; set; }

        public ClientSettingConfig()
        {
            this.ExcelMaxCountForInfo = 100000;
            this.ExcelSheetMaxCount = 50000;
            this.CheckBoxIsChecked = false;
        }
    }
}
