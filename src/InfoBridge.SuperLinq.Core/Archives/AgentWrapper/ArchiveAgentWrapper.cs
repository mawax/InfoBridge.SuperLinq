using System;
using SuperOffice.Services75;
using InfoBridge.SuperLinq.Core.Exceptions;

namespace InfoBridge.SuperLinq.Core.Archives.AgentWrapper
{
    public class ArchiveAgentWrapper : IArchiveAgentWrapper
    {
        private readonly ArchiveAgent _agent;

        public ArchiveAgentWrapper()
        {
            _agent = new ArchiveAgent();
        }

        public ArchiveListItem[] GetArchiveListByColumns(string providerName, string[] columns, ArchiveOrderByInfo[] sortOrder, ArchiveRestrictionInfo[] restriction, string[] entities, int page, int pageSize)
        {
            if (!SuperOffice.SoContext.IsAuthenticated)
            {
                throw new SuperLinqException("The current SuperOffice context is not authenticated. Make sure to authenticate in NetServer first.");
            }
            return _agent.GetArchiveListByColumns(providerName, columns, sortOrder, restriction, entities, page, pageSize);
        }

        public void Dispose()
        {
            _agent?.Dispose();
        }
    }
}
