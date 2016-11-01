using InfoBridge.SuperLinq.Core.Archives;
using InfoBridge.SuperLinq.Core.Archives.AgentWrapper;
using InfoBridge.SuperLinq.Core.Dependencies;
using InfoBridge.SuperLinq.Core.Projection;
using InfoBridge.SuperLinq.Core.QueryBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core
{
    public class SimpleQueryable<T> : ISimpleQueryable<T>
    {
        private readonly IArchiveManager<T> _archiveWrapper;

        public SimpleQueryable()
            : this(context: null) { }

        public SimpleQueryable(string archiveProviderName)
            : this(context: (context) => context.ArchiveName = archiveProviderName) { }

        public SimpleQueryable(Action<ArchiveExecutionContext> context)
            : this(DependencyHelper.CreateArchiveManager<T>(context)) { }

        public SimpleQueryable(IArchiveManager<T> archiveWrapper)
        {
            if (archiveWrapper == null) { throw new ArgumentNullException(nameof(archiveWrapper)); }
            _archiveWrapper = archiveWrapper;
        }

        public T GetFirst(RestrictionBuilderBase restrictionBuilder, OrderByBuilder orderByBuilder = null)
        {
            return _archiveWrapper.DoQuerySingle(restrictionBuilder, orderByBuilder);
        }

        public ICollection<T> GetList(RestrictionBuilderBase restrictionBuilder, OrderByBuilder orderByBuilder = null)
        {
            return _archiveWrapper.DoQuery(restrictionBuilder, orderByBuilder);
        }
    }
}
