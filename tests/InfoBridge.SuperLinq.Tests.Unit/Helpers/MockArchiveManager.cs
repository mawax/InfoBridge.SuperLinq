using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfoBridge.SuperLinq.Core.QueryBuilders;
using InfoBridge.SuperLinq.Core.Archives;

namespace InfoBridge.SuperLinq.Tests.Unit.Helpers
{
    public class MockArchiveManager<T> : IArchiveManager<T>
    {
        public RestrictionBuilderBase RestrictionBuilder { get; set; }
        public OrderByBuilder OrderByBuilder { get; set; }
        public int NoItems { get; set; }
        public int Page { get; set; }
        public bool DoQuerySingleCalled { get; set; }

        public IList<T> DoQuery(RestrictionBuilderBase restrictionBuilder, OrderByBuilder orderByBuilder = null, int noItems = 0, int page = 0)
        {
            this.RestrictionBuilder = restrictionBuilder;
            this.OrderByBuilder = orderByBuilder;
            this.NoItems = noItems;
            this.Page = page;
            return new List<T>();
        }

        public T DoQuerySingle(RestrictionBuilderBase restrictionBuilder, OrderByBuilder orderByBuilder = null)
        {
            DoQuerySingleCalled = true;
            this.RestrictionBuilder = restrictionBuilder;
            this.OrderByBuilder = orderByBuilder;
            return default(T);
        }
    }
}
