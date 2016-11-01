using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InfoBridge.SuperLinq.Core.Projection;

namespace InfoBridge.SuperLinq.Core.LinqProvider
{
    public class OrderTreeVisitor : ExpressionVisitorBase
    {
        private string _column;
        public string Column { get { return _column; } }

        protected override Expression VisitMember(MemberExpression expression)
        {
            _column = DynamicPropertyHelper.GetColumnName(expression.Member.DeclaringType, expression.Member.Name);
            return expression;
        }
    }
}
