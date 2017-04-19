using ExcelReaderExtension.Exceptions.Trap;
using ExcelReaderExtension.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExcelReaderExtension.Infrastructure.Parse
{
    public class NullableDateTimeParse : IParse<DateTime?>
    {
        private readonly object value;
        private readonly List<string> formats;

        public NullableDateTimeParse(object value, List<string> formats)
        {
            ThrowIfs.NullOrEmpty(formats, nameof(formats));

            this.value = value;
            this.formats = formats;
        }

        public DateTime? Get()
        {
            if (value == null)
            {
                return null;
            }

            DateTime outTime;
            var isSuccess = DateTime.TryParseExact(value.ToString(), formats.ToArray(), CultureInfo.InvariantCulture, DateTimeStyles.None, out outTime);
            return isSuccess == false ? (DateTime?)null : outTime;
        }
    }
}