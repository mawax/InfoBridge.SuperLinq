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
    /// <summary>
    /// LINQ provider for querying SuperOffice Web Services. This class is the main entry point for a LINQ query.
    /// </summary>
    /// <typeparam name="T">The type of the result items yielded by this query. 
    /// This type must be decorated with a TableInfo attribute and it's (public) properties must decorated by ColumnInfo attributes.</typeparam>
    public class Queryable<T> : QueryableBase<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Queryable{T}"/> class. The query will be executed using the dynamic archive provider.
        /// </summary>
        public Queryable()
            : this(context: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queryable{T}"/> class specifying a specific archive provider to use.
        /// </summary>
        /// <param name="archiveProviderName">Name of the archive provider to use, e.g. 'Dynamic', or 'ContactSelection'.</param>
        public Queryable(string archiveProviderName)
            : this(context: (context) => context.ArchiveName = archiveProviderName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queryable{T}"/> class specifying a specific archive provider to use and entities to include.
        /// If left out, all entities are included
        /// </summary>
        /// <param name="archiveProviderName">Name of the archive provider to use, e.g. 'Dynamic', or 'ContactSelection'.</param>
        /// <param name="entities">Which entities to include, if not provided, all entities are included</param>
        public Queryable(string archiveProviderName, params string[] entities)
            : this(context: (context) => { context.ArchiveName = archiveProviderName; context.Entities = entities; }) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queryable{T}"/> class and configure it using an execution context.
        /// </summary>
        /// <param name="context">Action to configure the <see cref="ArchiveExecutionContext"/>.</param>
        public Queryable(Action<ArchiveExecutionContext> context)
            : this(DependencyHelper.CreateTypedQueryExecutor<T>(context)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queryable{T}"/> class with a specific <see cref="ITypedQueryExecutor{T}"/> 
        /// and the default <see cref="IQueryParser"/>.
        /// This constructor should normally not be used.
        /// </summary>
        /// <param name="executor">The <see cref="ITypedQueryExecutor{T}"/> used to execute the query.</param>
        public Queryable(ITypedQueryExecutor<T> executor)
            : base(QueryParser.CreateDefault(), executor) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queryable{T}"/> class with a specific <see cref="IQueryProvider"/>.
        /// This constructor should normally not be used.
        /// </summary>
        /// <param name="provider">The <see cref="IQueryProvider"/> used to execute the query represented by this 
        /// <see cref="Queryable{T}"/> and to construct queries around this <see cref="Queryable{T}"/>.
        /// </param>
        public Queryable(IQueryProvider provider)
            : base(provider) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queryable{T}"/> class with a given provider and expression. 
        /// This is an infrastructure constructor that must be exposed because it is used by 
        /// <see cref="DefaultQueryProvider"/> to construct queries around this <see cref="Queryable{T}"/> when a 
        /// query method (e.g. of the <see cref="Queryable"/> class) is called.
        /// This constructor should normally not be used.
        /// </summary>
        /// <param name="provider">The <see cref="IQueryProvider"/> used to execute the query represented by this 
        /// <see cref="Queryable{T}"/> and to construct queries around this <see cref="Queryable{T}"/>.
        /// </param>
        /// <param name="expression">The expression representing the query.</param>
        public Queryable(IQueryProvider provider, Expression expression)
            : base(provider, expression) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queryable{T}"/> class with a <see cref="IQueryParser"/> and 
        /// the given executor. 
        /// This constructor should normally not be used.
        /// </summary>
        /// <param name="queryParser">The <see cref="IQueryParser"/> used to parse queries</param>
        /// <param name="executor">The <see cref="IQueryExecutor"/> used to execute the query.</param>
        public Queryable(IQueryParser queryParser, IQueryExecutor executor)
            : base(queryParser, executor) { }
    }
}
