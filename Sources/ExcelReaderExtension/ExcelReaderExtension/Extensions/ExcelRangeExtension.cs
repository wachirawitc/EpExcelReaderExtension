using ExcelReaderExtension.Infrastructure;
using ExcelReaderExtension.Infrastructure.Interface;
using OfficeOpenXml;
using System;
using System.Linq.Expressions;
using ExcelReaderExtension.Infrastructure.Converter;

namespace ExcelReaderExtension.Extensions
{
    public static class ExcelRangeExtension
    {
        public static IRuleBuilder<T> Cast<T>(this ExcelRange range)
        {
            return Cast(range, new DefaultConverter<T>(range, null, null));
        }

        public static IRuleBuilder<T> Cast<T>(this ExcelRange range, Expression<Func<ExcelRangeBase, string>> error)
        {
            return Cast(range, new DefaultConverter<T>(range, null, error));
        }

        public static IRuleBuilder<T> Cast<T>(this ExcelRange range, Expression<Func<object, T>> function)
        {
            return Cast(range, new DefaultConverter<T>(range, function, null));
        }

        public static IRuleBuilder<T> Cast<T>(this ExcelRange range, Expression<Func<object, T>> function, Expression<Func<ExcelRangeBase, string>> error)
        {
            return Cast(range, new DefaultConverter<T>(range, function, error));
        }

        public static IRuleBuilder<T> Cast<T>(this ExcelRange range, Expression<Func<object, IConverter<T>>> function)
        {
            var compile = function.Compile();

            return Cast(range, compile(range.Value));
        }

        public static IRuleBuilder<T> Cast<T>(this ExcelRange range, IConverter<T> converter)
        {
            return new RuleBuilder<T>(range, converter);
        }
    }
}