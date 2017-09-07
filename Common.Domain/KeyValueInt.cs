using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class KeyValueInt
    {
        public int Key { get; set; }
        public string Value { get; set; }

        public KeyValueInt(int key ,string value)
        {
            this.Key = key;
            this.Value = value;
        }

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
