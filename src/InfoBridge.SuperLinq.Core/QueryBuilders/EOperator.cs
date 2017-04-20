using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.QueryBuilders
{
    public enum EOperator
    {
        Equals,
        Greater,
        GreaterEqual,
        Less,
        LessEqual,
        UnEquals,
        Between,
        AllOf,
        OneOf,
        NotOneOf,
        Begins,
        Contains,
        Ends,
        Is,
        NotBegins,
        IsNot,
        NotContains,
        IsNull
    }
}
