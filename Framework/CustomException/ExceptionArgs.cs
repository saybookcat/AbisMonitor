using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CustomException
{
    [Serializable]
    public abstract class ExceptionArgs
    {
        public virtual String Message
        {
            get
            {
                return String.Empty;
            }
        }
    }
}
