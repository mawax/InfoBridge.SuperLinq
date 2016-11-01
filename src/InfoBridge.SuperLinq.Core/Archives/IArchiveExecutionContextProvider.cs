using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Archives
{
    public interface IArchiveExecutionContextProvider
    {
        ArchiveExecutionContext Get();
    }
}
