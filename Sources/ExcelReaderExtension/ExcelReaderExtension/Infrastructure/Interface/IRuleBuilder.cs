using ExcelReaderExtension.Infrastructure.Model;
using System;
using System.Linq.Expressions;

namespace ExcelReaderExtension.Infrastructure.Interface
{
    public interface IRuleBuilder<T>
    {
        IRuleBuilder<T> WithMessage(Expression<Func<CellResource, string>> message);

        IRuleBuilder<T> WithMessage(Expression<Func<CellResource, T, string>> message);

        IRuleBuilder<T> NotNull();

        IRuleBuilder<T> NumericOnly();

        IRuleBuilder<T> DecimalOnly();

        IRuleBuilder<T> NotEmpty();

        IRuleBuilder<T> Must(Expression<Func<T, bool>> condition);

        IRuleBuilder<T> Must(Expression<Func<bool>> condition);

        IRuleBuilder<T> Must(IValidationRule validationRule);

        IRuleBuilder<T> Contains(params T[] sources);

        T Get();
    }
}