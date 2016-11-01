using InfoBridge.SuperLinq.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.ModelBase
{
    public abstract class StandardModelBase : IStandardModel
    {
        public abstract int PK { get; set; }

        [ColumnInfo("registered", UseRaw = true)]
        public DateTime Registered { get; set; }

        [ColumnInfo("registered_associate_id")]
        public int RegisteredAssociateId { get; set; }

        [ColumnInfo("updated", UseRaw = true)]
        public DateTime Updated { get; set; }

        [ColumnInfo("updated_associate_id")]
        public int UpdatedAssociateId { get; set; }
    }
}
