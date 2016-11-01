using InfoBridge.SuperLinq.Core.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core
{
    public static class Settings
    {
        public static Action<ArchiveExecutionContext> DefaultExecutionContext { get; set; }
    }
}
