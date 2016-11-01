using InfoBridge.SuperLinq.Core.QueryBuilders;
using SuperOffice.Services75;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.LinqProvider
{
    public static class ExpressionTypeMap
    {
        public static Dictionary<ExpressionType, EOperator> OperatorMap { get; private set; } = CreateOperatorMap();
        public static Dictionary<string, EOperator> MethodOperatorMap { get; private set; } = CreateMethodOperatorMap();
        public static Dictionary<ExpressionType, InterRestrictionOperator> InterRestrictionOperatorMap { get; private set; } = CreateInterRestrictionOperatorMap();

        private static Dictionary<ExpressionType, EOperator> CreateOperatorMap()
        {
            var d = new Dictionary<ExpressionType, EOperator>();

            d.Add(ExpressionType.Equal, EOperator.Equals);
            d.Add(ExpressionType.GreaterThan, EOperator.Greater);
            d.Add(ExpressionType.GreaterThanOrEqual, EOperator.GreaterEqual);
            d.Add(ExpressionType.LessThan, EOperator.Less);
            d.Add(ExpressionType.LessThanOrEqual, EOperator.LessEqual);
            d.Add(ExpressionType.NotEqual, EOperator.UnEquals);

            return d;
        }

        private static Dictionary<string, EOperator> CreateMethodOperatorMap()
        {
            var d = new Dictionary<string, EOperator>();

            d.Add("System.String->Contains", EOperator.Contains);
            d.Add("System.String->StartsWith", EOperator.Begins);
            d.Add("System.String->EndsWith", EOperator.Ends);
            d.Add("System.String->Equals", EOperator.Equals);

            return d;
        }

        private static Dictionary<ExpressionType, InterRestrictionOperator> CreateInterRestrictionOperatorMap()
        {
            var d = new Dictionary<ExpressionType, InterRestrictionOperator>();

            d.Add(ExpressionType.AndAlso, InterRestrictionOperator.And);
            d.Add(ExpressionType.And, InterRestrictionOperator.And);
            d.Add(ExpressionType.OrElse, InterRestrictionOperator.Or);
            d.Add(ExpressionType.Or, InterRestrictionOperator.Or);

            return d;
        }
    }
}
