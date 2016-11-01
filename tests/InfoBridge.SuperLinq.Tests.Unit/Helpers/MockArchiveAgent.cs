using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperOffice.Services75;
using InfoBridge.SuperLinq.Core.Archives.AgentWrapper;

namespace InfoBridge.SuperLinq.Tests.Unit.Helpers
{
    public class MockArchiveAgent : IArchiveAgentWrapper
    {
        public int CallCount { get; private set; }

        private List<ArchiveListItem> dummyDatabase = new List<ArchiveListItem>();

        public MockArchiveAgent(int totalResults)
        {
            for (int i = 0; i < totalResults; i++)
            {
                dummyDatabase.Add(new ArchiveListItem());
            }
        }

        public ArchiveListItem[] GetArchiveListByColumns(string providerName, string[] columns, ArchiveOrderByInfo[] sortOrder, ArchiveRestrictionInfo[] restriction, string[] entities, int page, int pageSize)
        {
            CallCount++;

            int skipSize = page * pageSize;
            List<ArchiveListItem> result = dummyDatabase.Skip(skipSize).Take(pageSize).ToList();
            return result.ToArray();
        }

        public void Dispose()
        {
        }
    }
}
