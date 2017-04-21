using InfoBridge.SuperLinq.Core.Projection;
using SuperOffice.CRM.ArchiveLists;
using SuperOffice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.QueryBuilders
{
    /// <summary>
    /// Order by builder
    /// </summary>
    public class OrderByBuilder
    {
        private List<ArchiveOrderByInfo> _orders = new List<ArchiveOrderByInfo>();

        public OrderByBuilder()
        {
        }

        public void AddOrder(string fullColumnName, bool asc)
        {
            ArchiveOrderByInfo o = new ArchiveOrderByInfo();
            o.Name = fullColumnName;
            o.Direction = OrderBySortType.DESC;
            if (asc) { o.Direction = OrderBySortType.ASC; }

            _orders.Add(o);
        }

        public void AddOrder<T>(string column, bool asc)
        {
            string fullColumnName = DynamicPropertyHelper.GetFullDotSyntaxColumnName<T>(column);
            AddOrder(fullColumnName, asc);
        }

        public List<ArchiveOrderByInfo> Get()
        {
            return _orders;
        }
    }
}
