using InfoBridge.SuperLinq.Core.QueryBuilders;
using SuperOffice.Data;
using System.Linq.Expressions;

namespace InfoBridge.SuperLinq.Core.LinqProvider
{
    public class RestrictionExpressionTreeVisitor<T> : ExpressionVisitorBase
    {
        private class RBWrapper<N>
        {
            public string Property { get; set; }
            public EOperator Operator { get; set; }
            public object Value { get; set; }
            public int Level { get; set; }
        }

        private RBWrapper<T> _current = new RBWrapper<T>();
        private InterRestrictionOperator _lastInterOperator = InterRestrictionOperator.And;
        private LinqRestrictionBuilder<T> _result = new LinqRestrictionBuilder<T>();

        public LinqRestrictionBuilder<T> GetResult()
        {
            return _result;
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {
            if (IsLogicalOperator(expression.NodeType))
            {
                _current.Level++;
            }

            Visit(expression.Left);
            ProcessNodeType(expression);
            Visit(expression.Right);

            if (IsLogicalOperator(expression.NodeType))
            {
                _result.GetLast().InterParenthesis--;
            }
            else
            {
                AddCurrentRestriction();
            }

            return expression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            string methodName = expression.Method.DeclaringType + "->" + expression.Method.Name;

            if (ExpressionTypeMap.MethodOperatorMap.ContainsKey(methodName) &&
                expression.Arguments.Count == 1) //we currently only support methods with 1 parameter
            {
                Visit(expression.Object);
                _current.Operator = ExpressionTypeMap.MethodOperatorMap[methodName];
                Visit(expression.Arguments[0]);
                AddCurrentRestriction();
                return expression;
            }

            return base.VisitMethodCall(expression);
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            _current.Property = expression.Member.Name;
            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            _current.Value = expression.Value;
            return expression;
        }

        private void AddCurrentRestriction()
        {
            _result.Add(_current.Property, _current.Operator, _current.Level, _lastInterOperator, _current.Value);
            _current = new RBWrapper<T>();
        }

        private void ProcessNodeType(BinaryExpression expression)
        {
            if (IsLogicalOperator(expression.NodeType) && ExpressionTypeMap.InterRestrictionOperatorMap.ContainsKey(expression.NodeType))
            {
                _lastInterOperator = ExpressionTypeMap.InterRestrictionOperatorMap[expression.NodeType];
            }
            else if (ExpressionTypeMap.OperatorMap.ContainsKey(expression.NodeType))
            {
                // Check if this should be a null operator, else choose the mapped operator
                if (expression.NodeType == ExpressionType.Equal && (
                    (expression.Left is ConstantExpression && (expression.Left as ConstantExpression).Value == null) ||
                    (expression.Right is ConstantExpression && (expression.Right as ConstantExpression).Value == null)))
                    _current.Operator = EOperator.IsNull;
                else
                    _current.Operator = ExpressionTypeMap.OperatorMap[expression.NodeType];
            }
            else
            {
                //we don't know the nodeType and cannot handle it, throw exception by visiting base
                base.VisitBinary(expression);
            }
        }

        private bool IsLogicalOperator(ExpressionType nodeType)
        {
            return nodeType == ExpressionType.And || nodeType == ExpressionType.AndAlso || nodeType == ExpressionType.Or || nodeType == ExpressionType.OrElse;
        }
    }
}
