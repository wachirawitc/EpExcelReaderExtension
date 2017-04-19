using ExcelReaderExtension.Infrastructure.Interface;
using System.Collections;
using System.Linq;

namespace ExcelReaderExtension.Infrastructure.Validation.Rules
{
    public class NotEmptyRule : IValidationRule
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
            if (propertyValue is string)
            {
                return string.IsNullOrWhiteSpace(propertyValue as string);
            }
            return false;
        }
    }
}