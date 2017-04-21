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
    public class OrderByBuilderTests
    {
        [Fact]
        public void TestAddOrderGeneric()
        {
            var ob = new OrderByBuilder();
            ob.AddOrder<TestPerson>("name", true);
            ob.AddOrder<TestPerson>("id", false);

            List<ArchiveOrderByInfo> result = ob.Get();

            Assert.Equal("testperson.name", result[0].Name);
            Assert.Equal("testperson.id", result[1].Name);
            Assert.Equal(OrderBySortType.ASC, result[0].Direction);
            Assert.Equal(OrderBySortType.DESC, result[1].Direction);
        }

        [Fact]
        public void TestAddOrder()
        {
            var ob = new OrderByBuilder();
            ob.AddOrder("contact.name", true);
            ob.AddOrder("contact.id", false);

            List<ArchiveOrderByInfo> result = ob.Get();

            Assert.Equal("contact.name", result[0].Name);
            Assert.Equal("contact.id", result[1].Name);
            Assert.Equal(OrderBySortType.ASC, result[0].Direction);
            Assert.Equal(OrderBySortType.DESC, result[1].Direction);
        }
    }
}
