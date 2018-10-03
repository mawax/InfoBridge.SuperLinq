using SuperOffice.CRM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Archives
{
    public interface IResultParser
    {
        IList<T> Parse<T>(IList<ArchiveListItem> list);
        IList<T> Parse<T>(ArchiveExecutionContext executionContext, IList<ArchiveListItem> list);
    }
}
