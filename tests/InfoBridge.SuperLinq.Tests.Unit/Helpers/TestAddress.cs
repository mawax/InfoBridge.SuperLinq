using InfoBridge.SuperLinq.Core.Attributes;
using InfoBridge.SuperLinq.Core.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Tests.Unit.Helpers
{
    [TableInfo("testperson")]
    public class TestAddress : IModel
    {
        [ColumnInfo("id")]
        public int Id { get; set; }

        [ColumnInfo(configProviderClass: typeof(MockColumnConfigProvider), configProviderParameters: new[] { "Context1", "Context2" })]
        public string Test1 { get; set; }
    }
}
