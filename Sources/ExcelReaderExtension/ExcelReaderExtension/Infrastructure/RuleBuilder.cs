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
        private readonly CellResource resource;
        private readonly T source;
        private readonly ExcelRangeBase excelRange;
        private readonly List<ValidationContext<T>> validationContexts;

        public RuleBuilder(ExcelRangeBase excelRange, IConverter<T> converter)
        {
            this.excelRange = excelRange;
            source = converter.Get();

            validationContexts = new List<ValidationContext<T>>();

            resource = new CellResource
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
                    var message = context.DefaultMessage.Compile()(resource);

                    if (context.Message != null)
                    {
                        message = context.Message.Compile()(resource, source);
                    }

                    throw new ValidationException(message);
                }
            }

            return source;
        }

        public IRuleBuilder<T> WithMessage(Expression<Func<CellResource, string>> message)
        {
            if (validationContexts.Any())
            {
                validationContexts.Last().DefaultMessage = message;
            }
            return this;
        }

        public IRuleBuilder<T> WithMessage(Expression<Func<CellResource, T, string>> message)
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
                Rule = new DefaultExpressionRule(() => sources.Contains(source)),
                DefaultMessage = cell => $"{cell.Address} is not contains."
            });

            return this;
        }

        public IRuleBuilder<T> NotNull()
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new NotNullRule(excelRange.Value),
                DefaultMessage = cell => $"{cell.Address} is not null."
            });

            return this;
        }

        public IRuleBuilder<T> NumericOnly()
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new NumericOnlyRule(excelRange.Value),
                DefaultMessage = cell => $"{cell.Address} is not numeric."
            });

            return this;
        }

        public IRuleBuilder<T> DecimalOnly()
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new DecimalOnlyRule(excelRange.Value),
                DefaultMessage = detail => $"{detail.Address} is not decimal."
            });

            return this;
        }

        public IRuleBuilder<T> Must(Expression<Func<T, bool>> condition)
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new ExpressionRule<T>(source, condition),
                DefaultMessage = cell => $"{cell.Address} is invalid."
            });

            return this;
        }

        public IRuleBuilder<T> Must(Expression<Func<bool>> condition)
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new DefaultExpressionRule(condition),
                DefaultMessage = cell => $"{cell.Address} is invalid."
            });

            return this;
        }

        public IRuleBuilder<T> Must(IValidationRule validationRule)
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = validationRule,
                DefaultMessage = cell => $"{cell.Address} is invalid rule."
            });

            return this;
        }

        public IRuleBuilder<T> NotEmpty()
        {
            validationContexts.Add(new ValidationContext<T>
            {
                Rule = new NotEmptyRule(excelRange.Value),
                DefaultMessage = cell => $"{cell.Address} is not empty."
            });

            return this;
        }

        #endregion Rules
    }
}