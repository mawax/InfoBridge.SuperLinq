using InfoBridge.SuperLinq.Core.QueryBuilders;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using System;
using System.Linq;

namespace InfoBridge.SuperLinq.Core.LinqProvider
{
    public class QueryGeneration<T> : QueryModelVisitorBase
    {
        private readonly QueryBuilder<T> _queryBuilder;
        private readonly QueryModel _queryModel;

        public QueryGeneration(QueryModel queryModel)
        {
            if (queryModel == null) { throw new ArgumentNullException(nameof(queryModel)); }

            this._queryModel = queryModel;
            _queryBuilder = new QueryBuilder<T>();
        }

        public QueryBuilder<T> GetQuery()
        {
            VisitQueryModel(_queryModel);
            return _queryBuilder;
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            var treeVisitor = new RestrictionExpressionTreeVisitor<T>();
            treeVisitor.Visit(whereClause.Predicate);
            _queryBuilder.RestrictionBuilder = treeVisitor.GetResult();

            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            if (orderByClause.Orderings.Any())
            {
                if (_queryBuilder.OrderByBuilder == null) { _queryBuilder.OrderByBuilder = new OrderByBuilder(); }

                foreach (Ordering order in orderByClause.Orderings)
                {
                    var treeVisitor = new OrderTreeVisitor();
                    treeVisitor.Visit(order.Expression);
                    _queryBuilder.OrderByBuilder.AddOrder<T>(treeVisitor.Column, order.OrderingDirection == OrderingDirection.Asc);
                }
            }
            base.VisitOrderByClause(orderByClause, queryModel, index);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            var visitor = new ResultOperatorVisitor<T>(_queryBuilder);
            visitor.Visit(resultOperator);
            base.VisitResultOperator(resultOperator, queryModel, index);
        }
    }
}