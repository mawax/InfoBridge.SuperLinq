using SuperOffice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.QueryBuilders
{
    /// <summary>
    /// A very simple string based restriction builder that does not allow nested restrictions using parentheses
    /// </summary>
    public class StringRestrictionBuilder : RestrictionBuilderBase
    {
        public StringRestrictionBuilder And(string column, EOperator op, params object[] values)
        {
            return Add(column, op, InterRestrictionOperator.And, 0, values);
        }

        public StringRestrictionBuilder Or(string column, EOperator op, params object[] values)
        {
            return Add(column, op, InterRestrictionOperator.Or, 0, values);
        }

        private StringRestrictionBuilder Add(string column, EOperator op, InterRestrictionOperator andOr, int level, params object[] values)
        {
            AddRestriction(column, op, andOr, level, values);
            return this;
        }
    }
}
