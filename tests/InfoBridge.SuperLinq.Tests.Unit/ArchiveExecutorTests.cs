using InfoBridge.SuperLinq.Core.Archives;
using InfoBridge.SuperLinq.Core.Archives.AgentWrapper;
using InfoBridge.SuperLinq.Tests.Unit.Helpers;
using Moq;
using SuperOffice.CRM.ArchiveLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InfoBridge.SuperLinq.Tests.Unit
{
    public class ArchiveExecutorTests
    {
        [Theory]
        [InlineData(10, 20, 5, 0, 10, 3)] //expects 3 queries, because the last query notices that there are no further results
        [InlineData(44, 20, 200, 0, 20, 1)] //expects 1 quey, fetch all at once
        [InlineData(9, 20, 5, 0, 9, 2)] //expects 2 queries, because the last query returns less than the page size
        [InlineData(30, 15, 5, 0, 15, 3)] //3 queries, should then stop as the max is reached
        [InlineData(0, 15, 5, 0, 0, 1)] //1 query to see that there are no results
        [InlineData(17, 10, 10, 1, 7, 1)] //1 query because we only fetch page 1
        public void TestGetItems(int totalQueryResult, int maxFetchResult, int pageSize, int page, int expectedResultCount, int callCount)
        {
            var agent = new MockArchiveAgent(totalQueryResult);
            ArchiveExecutor executor = createExecutor(agent);

            var result = executor.GetItems(createDummyQueryParameters(pageSize), maxFetchResult, page);
            Assert.Equal(expectedResultCount, result.Count);
            Assert.Equal(callCount, agent.CallCount);
        }

        private ArchiveExecutor createExecutor(MockArchiveAgent agent)
        {
            return new ArchiveExecutor(() => agent);
        }

        private ArchiveQueryParameters createDummyQueryParameters(int pageSize)
        {
            var parameters = new ArchiveQueryParameters();
            parameters.PageSize = pageSize;
            parameters.RequestedColumns.Add("column1");
            parameters.Restrictions.Add(new ArchiveRestrictionInfo { Name = "column1", Operator = "equals", Values = new[] { "value1" } });
            return parameters;
        }

    }
}
