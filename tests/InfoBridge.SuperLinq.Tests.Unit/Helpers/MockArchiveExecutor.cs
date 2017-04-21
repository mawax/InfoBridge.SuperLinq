using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfoBridge.SuperLinq.Core.Archives;
using SuperOffice.CRM.Services;

namespace InfoBridge.SuperLinq.Tests.Unit.Helpers
{ 
    public class MockArchiveExecutor : IArchiveExecutor
    {
        public ArchiveQueryParameters Parameters { get; set; }
        public int MaxItems { get; set; }
        public int Page { get; set; }

        public IList<ArchiveListItem> GetItems(ArchiveQueryParameters parameters, int maxItems, int page)
        {
            this.Parameters = parameters;
            this.MaxItems = maxItems;
            this.Page = page;
            return new List<ArchiveListItem>();
        }
    }
}
