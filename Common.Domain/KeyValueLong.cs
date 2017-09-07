using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class KeyValueLong
    {
        public long Key { get; set; }

        public string Value { get; set; }

        private string _valueKey;
        public string ValueKey
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_valueKey) ? _valueKey : String.Format("{0}({1})", Value, Key);
            }
            set { _valueKey = value; }
        }

    }
}
