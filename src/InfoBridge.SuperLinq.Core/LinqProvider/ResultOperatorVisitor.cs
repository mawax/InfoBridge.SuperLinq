using InfoBridge.SuperLinq.Core.QueryBuilders;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.LinqProvider
{
    public class ResultOperatorVisitor<T>
    {
        private readonly QueryBuilder<T> _queryBuilder;

        public ResultOperatorVisitor(QueryBuilder<T> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public void Visit(ResultOperatorBase resultOperator)
        {
            if (resultOperator is FirstResultOperator)
            {
                Visit((FirstResultOperator)resultOperator);
            }
            else if (resultOperator is TakeResultOperator)
            {
                Visit((TakeResultOperator)resultOperator);
            }
            else if (resultOperator is SkipResultOperator)
            {
                Visit((SkipResultOperator)resultOperator);
            }
            else if (resultOperator is SingleResultOperator)
            {
                Visit((SingleResultOperator)resultOperator);
            }
            else
            {
                ThrowNotSupported(resultOperator);
            }
        }

        public void Visit(FirstResultOperator resultOperator)
        {
            _queryBuilder.GetFirst = true;
        }

        public void Visit(SingleResultOperator resultOperator)
        {
            _queryBuilder.GetSingle = true;
        }

        public void Visit(TakeResultOperator resultOperator)
        {
            if (resultOperator.Count.NodeType == ExpressionType.Constant)
            {
                _queryBuilder.Take = (int)((ConstantExpression)resultOperator.Count).Value;
            }
            else
            {
                throw new NotSupportedException("The Take clause only supports constant values. Methods and variables are currently not supported.");
            }
        }

        public void Visit(SkipResultOperator resultOperator)
        {
            if (resultOperator.Count.NodeType == ExpressionType.Constant)
            {
                _queryBuilder.Skip = (int)((ConstantExpression)resultOperator.Count).Value;
            }
            else
            {
                throw new NotSupportedException("The Skip clause only supports constant values. Methods and variables are currently not supported.");
            }
        }

        private void ThrowNotSupported(ResultOperatorBase resultOperator)
        {
            throw new NotSupportedException("Result operator is not supported by the SuperOffice Linq Provider: " + resultOperator.ToString());
        }
    }
}
