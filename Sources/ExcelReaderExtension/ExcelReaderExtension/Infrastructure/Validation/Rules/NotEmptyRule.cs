using ExcelReaderExtension.Infrastructure.Interface;
using System.Collections;
using System.Linq;

namespace ExcelReaderExtension.Infrastructure.Validation.Rules
{
    internal class NotEmptyRule : IValidationRule
    {
        private readonly object value;

        public NotEmptyRule(object value)
        {
            this.value = value;
        }

        public bool IsValid()
        {
            if (!(value == null
                || IsInvalidString(value)
                || IsEmptyCollection(value)))
            {
                return false;
            }

            return true;
        }

        private static bool IsEmptyCollection(object propertyValue)
        {
            var collection = propertyValue as IEnumerable;
            return collection != null && !collection.Cast<object>().Any();
        }

        private static bool IsInvalidString(object propertyValue)
        {
            var data = propertyValue as string;
            if (data != null)
            {
                return string.IsNullOrWhiteSpace(data);
            }
            return false;
        }
    }
}