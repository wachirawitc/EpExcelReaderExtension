using ExcelReaderExtension.Exceptions;
using ExcelReaderExtension.Infrastructure.Interface;
using ExcelReaderExtension.Infrastructure.Model;
using ExcelReaderExtension.Infrastructure.Validation;
using ExcelReaderExtension.Infrastructure.Validation.Rules;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExcelReaderExtension.Infrastructure
{
    public class RuleBuilder<T> : IRuleBuilder<T>
    {
        private readonly CellResource<T> resource;
        private readonly IConverter<T> converter;
        private readonly ExcelRangeBase excelRange;
        private readonly List<ValidationContext<T>> validationContexts;

        public RuleBuilder(ExcelRangeBase excelRange, IConverter<T> converter)
        {
            this.excelRange = excelRange;
            this.converter = converter;

            validationContexts = new List<ValidationContext<T>>();

            resource = new CellResource<T>
            {
                Row = excelRange.Rows,
                Column = excelRange.Columns,
                Address = excelRange.Address,
                WorksheetName = excelRange.Worksheet.Name
            };
        }

        public T Get()
        {
            foreach (var context in validationContexts)
            {
                if (context.Rule.IsValid() == false)
                {
                    var function = context.Message.Compile();
                    throw new ValidationException(function(resource));
                }
            }

            return converter.Get();
        }

        public IRuleBuilder<T> WithMessage(Expression<Func<CellResource<T>, string>> message)
        {
            if (validationContexts.Any())
            {
                validationContexts.Last().Message = message;
            }
            return this;
        }

        #region Rules

        public IRuleBuilder<T> Contains(params T[] sources)
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new DefaultExpressionRule(() => sources.Contains(converter.Get())),
                Message = cell => $"{cell.Address} is not contains."
            });

            return this;
        }

        public IRuleBuilder<T> NotNull()
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new NotNullRule(excelRange.Value),
                Message = cell => $"{cell.Address} is not null."
            });

            return this;
        }

        public IRuleBuilder<T> NumericOnly()
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new NumericOnlyRule(excelRange.Value),
                Message = cell => $"{cell.Address} is not numeric."
            });

            return this;
        }

        public IRuleBuilder<T> DecimalOnly()
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new DecimalOnlyRule(excelRange.Value),
                Message = detail => $"{detail.Address} is not decimal."
            });

            return this;
        }

        public IRuleBuilder<T> Must(Expression<Func<T, bool>> condition)
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new ExpressionRule<T>(converter.Get(), condition),
                Message = cell => $"{cell.Address} is invalid."
            });

            return this;
        }

        public IRuleBuilder<T> Must(Expression<Func<bool>> condition)
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new DefaultExpressionRule(condition),
                Message = cell => $"{cell.Address} is invalid."
            });

            return this;
        }

        public IRuleBuilder<T> Must(IValidationRule validationRule)
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = validationRule,
                Message = cell => $"{cell.Address} is invalid rule."
            });

            return this;
        }

        public IRuleBuilder<T> NotEmpty()
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new NotEmptyRule(excelRange.Value),
                Message = cell => $"{cell.Address} is not empty."
            });

            return this;
        }

        #endregion Rules
    }
}