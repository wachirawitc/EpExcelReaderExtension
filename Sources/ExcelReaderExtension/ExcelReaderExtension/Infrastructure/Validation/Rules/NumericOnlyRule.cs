using ExcelReaderExtension.Infrastructure.Interface;

namespace ExcelReaderExtension.Infrastructure.Validation.Rules
{
    public class NumericOnlyRule : IValidationRule
    {
        private readonly object value;

        public NumericOnlyRule(object value)
        {
            this.value = value;
        }

        public bool IsValid()
        {
            bool isValid = value is byte ||
                   value is short ||
                   value is int ||
                   value is long;

            if (isValid == false && value != null)
            {
                int target;
                isValid = int.TryParse(value.ToString(), out target);
            }

            return isValid;
        }
    }
}