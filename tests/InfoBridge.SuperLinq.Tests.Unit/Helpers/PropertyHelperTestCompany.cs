using InfoBridge.SuperLinq.Core.Attributes;
using InfoBridge.SuperLinq.Core.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Tests.Unit.Helpers
{
    [TableInfo("testcompany")]
    public class PropertyHelperTestCompany : ISoModel
    {
        [ColumnInfo("id")]
        public int PK { get; set; }

        [ColumnInfo("id")]
        public int Id { get; set; }

        [ColumnInfo("name")]
        public string Name { get; set; }
            
        [ColumnInfo("activated")]
        public DateTime Activated { get; set; }

        [ColumnInfo("activated", UseRaw = true)]
        public DateTime ActivatedRaw { get; set; }
    }
}
