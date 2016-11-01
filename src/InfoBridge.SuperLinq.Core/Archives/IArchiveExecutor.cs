using SuperOffice.Services75;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Archives
{
    public interface IArchiveExecutor
    {
        IList<ArchiveListItem> GetItems(ArchiveQueryParameters parameters, int maxItems, int page);
    }
}
