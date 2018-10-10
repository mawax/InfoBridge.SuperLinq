using SuperOffice.CRM;
using SuperOffice.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Archives
{
    public class ArchiveExecutionContext
    {
        public string ArchiveName { get; set; } = "Dynamic";
        public string[] Entities { get; set; }
        public bool DateTimeToUTC { get; set; }
    }
}
