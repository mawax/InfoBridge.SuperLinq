using InfoBridge.SuperLinq.Core.Attributes;
using InfoBridge.SuperLinq.Core.Projection;
using SuperOffice.CRM.Globalization;
using SuperOffice.CRM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Archives
{
    public class ResultParser : IResultParser
    {
        private readonly IDateTimeConverter _dateTimeConverter;

        public ResultParser() : this(new DateTimeConverter())
        {
            //constructor for convenient consuming
        }

        public ResultParser(IDateTimeConverter dateTimeConverter)
        {
            if (dateTimeConverter == null) { throw new ArgumentNullException(nameof(dateTimeConverter)); }
            _dateTimeConverter = dateTimeConverter;
        }

        public IList<T> Parse<T>(IList<ArchiveListItem> list)
        {
            return Parse<T>(new ArchiveExecutionContext(), list);
        }

        public IList<T> Parse<T>(ArchiveExecutionContext executionContext, IList<ArchiveListItem> list)
        {
            List<T> results = new List<T>();
            if (list == null) { return null; }

            foreach (ArchiveListItem item in list)
            {
                T t = Activator.CreateInstance<T>();
                foreach (string column in item.ColumnData.Keys)
                {
                    foreach (string propertyName in DynamicPropertyHelper.GetPropertyNames<T>(column))
                    {
                        if (item.ColumnData[column] != null)
                        {
                            string value = item.ColumnData[column].DisplayValue;
                            object parsedValue = CultureDataFormatter.ParseEncoded(value);

                            parsedValue = DoTypeSpecificConversion(executionContext, DynamicPropertyHelper.GetColumnInfo(typeof(T), propertyName), parsedValue);
                            ObjectPropertyAccessor.SetValue(t, propertyName, parsedValue);
                        }
                    }
                }
                results.Add(t);
            }
            return results;
        }

        private object DoTypeSpecificConversion(ArchiveExecutionContext executionContext, ColumnInfo column, object parsedValue)
        {
            if (parsedValue != null)
            {
                var type = parsedValue.GetType();
                if (type == typeof(DateTime))
                {
                    if (!executionContext.DateTimeToUTC || column.UseRaw)
                    {
                        parsedValue = (DateTime)parsedValue;
                    }
                    else
                    {
                        parsedValue = _dateTimeConverter.ConvertFromTimeZone(((DateTime)parsedValue).ToUniversalTime());
                    }
                }
                else if (column.PropertyType == typeof(int[]))
                {
                    //fix for incorrect value from SuperOffice Services
                    if (parsedValue as string == "[A:]") { parsedValue = new int[0]; }
                }
            }
            return parsedValue;
        }
    }
}
