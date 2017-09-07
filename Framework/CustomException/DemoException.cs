using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CustomException
{
    //定义一个完整的Demo的异常类  
    [Serializable]
    public sealed class DemoException : ExceptionArgs
    {
        //readonly:只读，动态常量，只能在构造器中被赋值  
        private readonly String _demoParam;

        public DemoException(String diskpath)
        {
            _demoParam = diskpath;
        }

        public String DemoParam
        {
            get { return _demoParam; }
        }

        public override string Message
        {
            get { return (_demoParam == null) ? base.Message : "demoParam=" + _demoParam; }
        }
    }
}
