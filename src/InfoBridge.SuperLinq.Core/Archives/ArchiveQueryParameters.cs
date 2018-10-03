using SuperOffice.CRM.ArchiveLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Archives
{
    public class ArchiveQueryParameters
    {
        public const int MAX_PAGE_SIZE = 1000;
        public const int DEFAULT_PAGE_SIZE = 200;

        public IList<string> RequestedColumns { get; set; }
        public string ArchiveName { get; set; }
        public IList<ArchiveRestrictionInfo> Restrictions { get; set; }
        public List<ArchiveOrderByInfo> OrderBy { get; set; }
        public string[] Entities { get; set; }
        public int PageSize { get; set; }

        public ArchiveQueryParameters()
        {
            Restrictions = new List<ArchiveRestrictionInfo>();
            RequestedColumns = new List<string>();
            OrderBy = new List<ArchiveOrderByInfo>();
            PageSize = DEFAULT_PAGE_SIZE;
        }
    }
}
