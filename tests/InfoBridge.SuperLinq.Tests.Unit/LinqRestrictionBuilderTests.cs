using InfoBridge.SuperLinq.Core.QueryBuilders;
using InfoBridge.SuperLinq.Tests.Unit.Helpers;
using SuperOffice.CRM.ArchiveLists;
using SuperOffice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InfoBridge.SuperLinq.Tests.Unit
{
    public class LinqRestrictionBuilderTests
    {
        [Fact]
        public void TestAdd()
        {
            var builder = new LinqRestrictionBuilder<TestPerson>();
            builder.Add("Id", EOperator.Greater, 1, InterRestrictionOperator.And, 1000);
            builder.Add("FirstName", EOperator.Equals, -1, InterRestrictionOperator.Or, "InfoBridge");

            List<ArchiveRestrictionInfo> results = builder.GetRestrictions();
            Assert.NotEmpty(results);
            Assert.Equal(2, results.Count);
            Assert.Equal("testperson.id", results[0].Name);
            Assert.Equal(new[] { "[I:1000]" }, results[0].Values);
            Assert.Equal("greater", results[0].Operator);
            Assert.Equal(1, results[0].InterParenthesis);
            Assert.Equal(new[] { "InfoBridge" }, results[1].Values);
            Assert.Equal(InterRestrictionOperator.Or, results[0].InterOperator); //we change the previous so this should be OR
        }
    }
}
