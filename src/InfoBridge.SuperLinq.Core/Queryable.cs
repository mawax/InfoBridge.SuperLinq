using InfoBridge.SuperLinq.Core.Archives;
using InfoBridge.SuperLinq.Core.Dependencies;
using InfoBridge.SuperLinq.Core.LinqProvider;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core
{
    public class Queryable<T> : QueryableBase<T>
    {
        public Queryable()
            : this(context: null) { }

        public Queryable(string archiveProviderName)
            : this(context: (context) => context.ArchiveName = archiveProviderName) { }

        public Queryable(Action<ArchiveExecutionContext> context)
            : this(DependencyHelper.CreateTypedQueryExecutor<T>(context)) { }

        public Queryable(ITypedQueryExecutor<T> executor)
            : base(QueryParser.CreateDefault(), executor) { }

        public Queryable(IQueryProvider provider)
            : base(provider) { }

        public Queryable(IQueryProvider provider, Expression expression)
            : base(provider, expression) { }

        public Queryable(IQueryParser queryParser, IQueryExecutor executor)
            : base(queryParser, executor) { }
    }
}
