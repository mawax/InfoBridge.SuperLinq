using InfoBridge.SuperLinq.Core.QueryBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Archives
{
    public interface IArchiveManager<T>
    {
        T DoQuerySingle(RestrictionBuilderBase restrictionBuilder, OrderByBuilder orderByBuilder = null);
        IList<T> DoQuery(RestrictionBuilderBase restrictionBuilder, OrderByBuilder orderByBuilder = null, int noItems = 0, int page = 0);
    }
}
