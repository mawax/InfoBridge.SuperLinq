using InfoBridge.SuperLinq.Core.Projection;
using SuperOffice.Services75;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.QueryBuilders
{
    public class LinqRestrictionBuilder<T> : RestrictionBuilderBase
    {
        public void Add(string property, EOperator op, int level, InterRestrictionOperator interOperator, object value)
        {
            string column = DynamicPropertyHelper.GetColumnName<T>(property);
            AddRestriction<T>(column, op, interOperator, level, value);
        }
    }
}
