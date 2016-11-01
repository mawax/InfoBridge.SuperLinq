using SuperOffice.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Projection
{
    public interface IDateTimeConverter
    {
        DateTime ConvertFromTimeZone(DateTime fromConvertTimeUTC);
    }
}
