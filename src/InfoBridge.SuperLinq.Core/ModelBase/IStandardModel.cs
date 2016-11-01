using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.ModelBase
{
    public interface IStandardModel : ISoModel
    {
        DateTime Registered { get; set; }
        DateTime Updated { get; set; }
        int RegisteredAssociateId { get; set; }
        int UpdatedAssociateId { get; set; }
    }
}
