using InfoBridge.SuperLinq.Core.Archives;
using InfoBridge.SuperLinq.Core.QueryBuilders;
using Remotion.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.LinqProvider
{
    public class QueryExecutor<T> : ITypedQueryExecutor<T>
    {
        private readonly IArchiveManager<T> _archiveWrapper;

        public QueryExecutor(IArchiveManager<T> archiveWrapper)
        {
            if (archiveWrapper == null) { throw new ArgumentNullException(nameof(archiveWrapper)); }
            _archiveWrapper = archiveWrapper;
        }
        
        public IEnumerable<N> ExecuteCollection<N>(QueryModel queryModel)
        {
            QueryBuilder<N> builder = new QueryGeneration<N>(queryModel).GetQuery();
            return GetArchiveManager<N>().DoQuery(builder.RestrictionBuilder, builder.OrderByBuilder, builder.Take, builder.Skip);
        }

        public N ExecuteSingle<N>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            QueryBuilder<N> builder = new QueryGeneration<N>(queryModel).GetQuery();

            if (builder.Take > 0)
            {
                throw new NotSupportedException("Unable to specify Take when using fetching single record.");
            }

            if (builder.GetSingle)
            {
                return DoGetSingle<N>(builder, returnDefaultWhenEmpty);
            }
            else if (builder.GetFirst)
            {
                return DoGetFirst<N>(builder, returnDefaultWhenEmpty);
            }
            else
            {
                throw new InvalidOperationException("Unable to ExecuteSingle without specifying GetFirst or GetSingle.");
            }
        }

        private N DoGetSingle<N>(QueryBuilder<N> builder, bool returnDefaultWhenEmpty)
        {
            //GetSingle(OrDefault) should return one record if one is found and throw an exception when multiple results are found.
            //therefore we fetch 2 results and use default .net linq implementation to sort our the result
            var result = GetArchiveManager<N>().DoQuery(builder.RestrictionBuilder, builder.OrderByBuilder, noItems: 2);
            if (returnDefaultWhenEmpty)
            {
                return result.SingleOrDefault();
            }
            else
            {
                return result.Single();
            }
        }

        private N DoGetFirst<N>(QueryBuilder<N> builder, bool returnDefaultWhenEmpty)
        {
            //GetFirst(OrDefault) should, if there are results, always return the first.
            var result = GetArchiveManager<N>().DoQuerySingle(builder.RestrictionBuilder, builder.OrderByBuilder);
            if (returnDefaultWhenEmpty)
            {
                //where returning default when empty we can just return the value from the manager
                return result;
            }
            else
            {
                if (result == null)
                {
                    //just use the default .net linq implementation exception. This line will throw an exception
                    return new N[0].Single();
                }
                else
                {
                    return result;
                }
            }
        }

        public N ExecuteScalar<N>(QueryModel queryModel)
        {
            throw new NotSupportedException("ExecuteScalar is not supported.");
        }

        private IArchiveManager<N> GetArchiveManager<N>()
        {
            if (typeof(N).FullName != typeof(T).FullName)
            {
                throw new NotSupportedException("The inner type N must match the class type T. This implementation is forced on us " +
                    "by the combination of the ReLinq IQueryExecutor interface and our constructor dependency injection.");
            }
            return (IArchiveManager<N>)_archiveWrapper;
        }
    }
}
