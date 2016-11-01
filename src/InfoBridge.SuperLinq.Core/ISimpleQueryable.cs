using InfoBridge.SuperLinq.Core.QueryBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core
{
    public interface ISimpleQueryable<T>
    {
        T GetFirst(RestrictionBuilderBase restrictionBuilder, OrderByBuilder orderByBuilder = null);
        ICollection<T> GetList(RestrictionBuilderBase restrictionBuilder, OrderByBuilder orderByBuilder = null);
    }
}
