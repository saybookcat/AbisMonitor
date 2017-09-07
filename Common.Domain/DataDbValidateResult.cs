using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public enum DataDbValidateResult
    {
        Add,
        Update,
        Repeat,
        Conflict,
        Ignore
    }
}
