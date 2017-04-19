using ExcelReaderExtension.Infrastructure.Interface;
using System;
using System.Linq.Expressions;

namespace ExcelReaderExtension.Infrastructure.Validation.Rules
{
    public class ExpressionRule<T> : IValidationRule
    {
        private readonly T input;
        private readonly Expression<Func<T, bool>> condition;

        public ExpressionRule(T input, Expression<Func<T, bool>> condition)
        {
            this.input = input;
            this.condition = condition;
        }

        public bool IsValid()
        {
            var func = condition.Compile();
            return func(input);
        }
    }
}