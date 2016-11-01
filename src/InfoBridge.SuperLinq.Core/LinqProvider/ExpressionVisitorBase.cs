using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.LinqProvider
{
    public abstract class ExpressionVisitorBase : ThrowingExpressionVisitor
    {
        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            string itemText = FormatUnhandledItem(unhandledItem);
            var message = string.Format("The expression '{0}' (type: {1}) is not supported by the SuperOffice LINQ provider.", itemText, typeof(T));
            return new NotSupportedException(message);
        }

        private static string FormatUnhandledItem<T>(T unhandledItem)
        {
            var itemAsExpression = unhandledItem as Expression;
            return itemAsExpression != null ? itemAsExpression.ToString() : unhandledItem.ToString();
        }
    }
}
