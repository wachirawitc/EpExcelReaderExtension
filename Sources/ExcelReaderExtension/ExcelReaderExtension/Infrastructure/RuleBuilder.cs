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
        private readonly Cell model;
        private readonly IParse<T> parse;
        private readonly ExcelRangeBase excelRange;
        private readonly List<ValidationContext> validations;

        public RuleBuilder(ExcelRangeBase excelRange, IParse<T> parse)
        {
            this.excelRange = excelRange;
            this.parse = parse;

            validations = new List<ValidationContext>();

            model = new Cell
            {
                Row = excelRange.Rows,
                Column = excelRange.Columns,
                Address = excelRange.Address,
                WorksheetName = excelRange.Worksheet.Name
            };
        }

        public IRuleBuilder<T> Must(IValidationRule validationRule)
        {
            validations.Add(new ValidationContext
            {
                Rule = validationRule,
                Message = detail => $"Row {detail.Row} and Column {detail.Column} is invalid rule."
            });

            return this;
        }

        public IRuleBuilder<T> Contains(params T[] sources)
        {
            validations.Add(new ValidationContext
            {
                Rule = new DefaultExpressionRule(() => sources.Contains(parse.Get())),
                Message = detail => $"Row {detail.Row} and Column {detail.Column} is not contains."
            });

            return this;
        }

        public T Get()
        {
            foreach (var validation in validations)
            {
                if (validation.Rule.IsValid() == false)
                {
                    var func = validation.Message.Compile();
                    var message = func(model);
                    throw new ValidationErrorException(message);
                }
            }

            return parse.Get();
        }

        public IRuleBuilder<T> WithMessage(Expression<Func<Cell, string>> message)
        {
            if (validations.Any())
            {
                validations.Last().Message = message;
            }
            return this;
        }

        public IRuleBuilder<T> NotNull()
        {
            validations.Add(new ValidationContext
            {
                Rule = new NotNullRule(excelRange.Value),
                Message = detail => $"Row {detail.Row} and Column {detail.Column} is not null."
            });

            return this;
        }

        public IRuleBuilder<T> NumericOnly()
        {
            validations.Add(new ValidationContext
            {
                Rule = new NumericOnlyRule(excelRange.Value),
                Message = detail => $"Row {detail.Row} and Column {detail.Column} is numeric only."
            });

            return this;
        }

        public IRuleBuilder<T> DecimalOnly()
        {
            validations.Add(new ValidationContext
            {
                Rule = new DecimalOnlyRule(excelRange.Value),
                Message = detail => $"Row {detail.Row} and Column {detail.Column} is decimal only."
            });

            return this;
        }

        public IRuleBuilder<T> Must(Expression<Func<T, bool>> condition)
        {
            validations.Add(new ValidationContext
            {
                Rule = new ExpressionRule<T>(parse.Get(), condition),
                Message = detail => $"Row {detail.Row} and Column {detail.Column} is invalid."
            });

            return this;
        }

        public IRuleBuilder<T> Must(Expression<Func<bool>> condition)
        {
            validations.Add(new ValidationContext
            {
                Rule = new DefaultExpressionRule(condition),
                Message = detail => $"Row {detail.Row} and Column {detail.Column} is invalid."
            });

            return this;
        }

        public IRuleBuilder<T> NotEmpty()
        {
            validations.Add(new ValidationContext
            {
                Rule = new NotEmptyRule(excelRange.Value),
                Message = detail => $"Row {detail.Row} and Column {detail.Column} is empty."
            });

            return this;
        }
    }
}