using ExcelReaderExtension.Infrastructure.Interface;
using System;
using System.Linq.Expressions;

namespace ExcelReaderExtension.Infrastructure.Validation.Rules
{
    public class ExpressionRule<T> : IValidationRule
    {
        private readonly T value;
        private readonly Expression<Func<T, bool>> condition;

        public ExpressionRule(T value, Expression<Func<T, bool>> condition)
        {
            this.value = value;
            this.condition = condition;
        }

        public bool IsValid()
        {
            var func = condition.Compile();
            return func(value);
        }
    }
}