using SuperOffice.Services75;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Archives.AgentWrapper
{
    public interface IArchiveAgentWrapper : IDisposable
    {
        ArchiveListItem[] GetArchiveListByColumns(string providerName, string[] columns, ArchiveOrderByInfo[] sortOrder, ArchiveRestrictionInfo[] restriction, string[] entities, int page, int pageSize);
    }
}
