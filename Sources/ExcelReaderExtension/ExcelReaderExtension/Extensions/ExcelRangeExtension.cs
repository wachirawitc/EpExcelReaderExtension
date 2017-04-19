using ExcelReaderExtension.Infrastructure;
using ExcelReaderExtension.Infrastructure.Interface;
using ExcelReaderExtension.Infrastructure.Parse;
using OfficeOpenXml;
using System;
using System.Linq.Expressions;

namespace ExcelReaderExtension.Extensions
{
    public static class ExcelRangeExtension
    {
        public static IRuleBuilder<T> Cast<T>(this ExcelRange range)
        {
            return Cast(range, new DefaultParse<T>(range, null, null));
        }

        public static IRuleBuilder<T> Cast<T>(this ExcelRange range, Expression<Func<ExcelRangeBase, string>> error)
        {
            return Cast(range, new DefaultParse<T>(range, null, error));
        }

        public static IRuleBuilder<T> Cast<T>(this ExcelRange range, Expression<Func<object, T>> function)
        {
            return Cast(range, new DefaultParse<T>(range, function, null));
        }

        public static IRuleBuilder<T> Cast<T>(this ExcelRange range, Expression<Func<object, T>> function, Expression<Func<ExcelRangeBase, string>> error)
        {
            return Cast(range, new DefaultParse<T>(range, function, error));
        }

        public static IRuleBuilder<T> Cast<T>(this ExcelRange range, Expression<Func<object, IParse<T>>> function)
        {
            var compile = function.Compile();

            return Cast(range, compile(range.Value));
        }

        public static IRuleBuilder<T> Cast<T>(this ExcelRange range, IParse<T> parse)
        {
            return new RuleBuilder<T>(range, parse);
        }
    }
}