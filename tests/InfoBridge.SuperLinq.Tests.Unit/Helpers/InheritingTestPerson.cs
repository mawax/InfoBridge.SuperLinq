using InfoBridge.SuperLinq.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Tests.Unit.Helpers
{
    [TableInfo("inheritingtestperson")]
    public class InheritingTestPerson : TestPerson
    {
        [ColumnInfo("newid")]
        public override int Id { get; set; }

        public string ExtraProperty { get; set; }
    }
}
