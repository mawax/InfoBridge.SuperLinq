using System;

namespace InfoBridge.SuperLinq.Core.QueryBuilders
{
    public class QueryBuilder<T>
    {
        public RestrictionBuilderBase RestrictionBuilder { get; set; }
        public OrderByBuilder OrderByBuilder { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }

        public bool GetSingle { get; set; }
        public bool GetFirst { get; set; }
    }
}
