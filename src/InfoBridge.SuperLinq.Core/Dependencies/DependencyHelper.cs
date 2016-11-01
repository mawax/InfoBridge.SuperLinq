using InfoBridge.SuperLinq.Core.Archives;
using InfoBridge.SuperLinq.Core.Archives.AgentWrapper;
using InfoBridge.SuperLinq.Core.LinqProvider;
using InfoBridge.SuperLinq.Core.Projection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Dependencies
{
    /// <summary>
    /// Poor man's dependecy injection
    /// </summary>
    static class DependencyHelper
    {
        internal static IArchiveManager<T> CreateArchiveManager<T>(Action<ArchiveExecutionContext> context)
        {
            return new ArchiveManager<T>(
                new ArchiveExecutor(() => new ArchiveAgentWrapper()),
                new DateTimeConverter(),
                new ArchiveExecutionContextProvider(context)
            );
        }

        internal static ITypedQueryExecutor<T> CreateTypedQueryExecutor<T>(Action<ArchiveExecutionContext> context)
        {
            return new QueryExecutor<T>(CreateArchiveManager<T>(context));
        }
    }
}
