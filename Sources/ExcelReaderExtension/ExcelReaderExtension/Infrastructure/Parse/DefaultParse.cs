using ExcelReaderExtension.Exceptions;
using ExcelReaderExtension.Infrastructure.Interface;
using OfficeOpenXml;
using System;
using System.Linq.Expressions;

namespace ExcelReaderExtension.Infrastructure.Parse
{
    public class DefaultParse<T> : IParse<T>
    {
        private readonly ExcelRangeBase excelRange;
        private readonly Expression<Func<object, T>> function;
        private readonly Expression<Func<ExcelRangeBase, string>> errorMessage;

        public DefaultParse(ExcelRangeBase excelRange,
            Expression<Func<object, T>> function,
            Expression<Func<ExcelRangeBase, string>> errorMessage)
        {
            this.excelRange = excelRange;
            this.function = function;
            this.errorMessage = errorMessage;
        }

        public T Get()
        {
            try
            {
                T source;
                if (function != null)
                {
                    var func = function.Compile();
                    source = func(excelRange.Value);
                }
                else
                {
                    source = excelRange.GetValue<T>();
                }
                return source;
            }
            catch (Exception)
            {
                var message = $"{excelRange.Address} not supported cast type.";
                if (errorMessage != null)
                {
                    var func = errorMessage.Compile();
                    message = func(excelRange);
                }

                throw new NotSupportedCastTypeException(message);
            }
        }
    }
}