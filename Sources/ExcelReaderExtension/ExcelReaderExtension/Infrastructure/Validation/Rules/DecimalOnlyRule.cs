using ExcelReaderExtension.Infrastructure.Interface;

namespace ExcelReaderExtension.Infrastructure.Validation.Rules
{
    internal class DecimalOnlyRule : IValidationRule
    {
        private readonly object value;

        public DecimalOnlyRule(object value)
        {
            this.value = value;
        }

        public bool IsValid()
        {
            bool isValid = value is float ||
                   value is double ||
                   value is decimal;

            if (isValid == false && value != null)
            {
                decimal target;
                isValid = decimal.TryParse(value.ToString(), out target);
            }

            return isValid;
        }
    }
}