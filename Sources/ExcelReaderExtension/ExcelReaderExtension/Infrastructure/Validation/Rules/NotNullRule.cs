using ExcelReaderExtension.Infrastructure.Interface;

namespace ExcelReaderExtension.Infrastructure.Validation.Rules
{
    public class NotNullRule : IValidationRule
    {
        private readonly object value;

        public NotNullRule(object value)
        {
            this.value = value;
        }

        public bool IsValid()
        {
            return value != null;
        }
    }
}