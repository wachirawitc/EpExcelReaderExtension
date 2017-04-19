using ExcelReaderExtension.Infrastructure.Interface;
using System;
using System.Linq.Expressions;

namespace ExcelReaderExtension.Infrastructure.Validation.Rules
{
    public class DefaultExpressionRule : IValidationRule
    {
        private readonly Expression<Func<bool>> condition;

        public DefaultExpressionRule(Expression<Func<bool>> condition)
        {
            this.condition = condition;
        }

        public bool IsValid()
        {
            var func = condition.Compile();
            return func();
        }
    }
}