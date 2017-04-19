using ExcelReaderExtension.Exceptions;
using ExcelReaderExtension.Infrastructure.Interface;
using OfficeOpenXml;
using System;
using System.Linq.Expressions;

namespace ExcelReaderExtension.Infrastructure.Converter
{
    internal class DefaultConverter<T> : IConverter<T>
    {
        private readonly ExcelRangeBase excelRange;
        private readonly Expression<Func<object, T>> function;
        private readonly Expression<Func<ExcelRangeBase, string>> errorMessage;

        public DefaultConverter(ExcelRangeBase excelRange,
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
                var message = $"{excelRange.Address} cannot convert value.";
                if (errorMessage != null)
                {
                    var func = errorMessage.Compile();
                    message = func(excelRange);
                }

                throw new ConverterException(message);
            }
        }
    }
}