using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Archives
{
    public class ArchiveExecutionContextProvider : IArchiveExecutionContextProvider
    {
        private readonly ArchiveExecutionContext _context;

        public ArchiveExecutionContextProvider(Action<ArchiveExecutionContext> context = null)
        {
            var executionContext = new ArchiveExecutionContext();
            Settings.DefaultExecutionContext?.Invoke(executionContext);
            context?.Invoke(executionContext);
            _context = executionContext;
        }

        public ArchiveExecutionContext Get()
        {
            return _context;
        }
    }
}
