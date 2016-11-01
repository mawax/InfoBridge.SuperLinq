using SuperOffice;
using SuperOffice.CRM;
using SuperOffice.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Projection
{
    public class DateTimeConverter : IDateTimeConverter
    {
        /// <summary>
        /// Convert the SuperOffice database time to UTC time
        /// </summary>
        /// <param name="fromConvertTimeUTC"></param>
        /// <returns></returns>
        public DateTime ConvertFromTimeZone(DateTime fromConvertTimeUTC)
        {
            //convert the UTC time to local time first
            DateTime fromConvertTime = fromConvertTimeUTC.ToLocalTime();

            if (fromConvertTime == DateTime.MinValue || fromConvertTime == DateTime.MaxValue)
            {
                return fromConvertTime;
            }
            int tZOffset = GetTenantDataBaseTimeZone().GetTZOffset(fromConvertTime);
            DateTime convertedTime = fromConvertTime.AddMinutes(-((double)(tZOffset * 1)));
            
            //create utc
            return new DateTime(convertedTime.Ticks, DateTimeKind.Utc);
        }

        private TimeZoneData GetTenantDataBaseTimeZone()
        {
            if (SoContext.CurrentPrincipal == null) {
                throw new Exception("Unable to retrieve timezonedata from current NetServer session. No current principal found.");
            }
            return TimeConverter.GetTimeZoneDataById(SoContext.CurrentPrincipal.TimeZone);
        }
    }
}
