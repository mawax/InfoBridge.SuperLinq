using InfoBridge.SuperLinq.Core.Projection;
using InfoBridge.SuperLinq.Tests.Unit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InfoBridge.SuperLinq.Tests.Unit
{
    public class ColumnInfoTests
    {
        [Fact]
        public void TestColumnConfigProvider()
        {
            string test1Column = DynamicPropertyHelper.GetColumnName(typeof(TestAddress), nameof(TestAddress.Test1));
            Assert.Equal(MockColumnConfigProvider.SUCCESS_COLUMN_NAME, test1Column);
        }
    }
}
