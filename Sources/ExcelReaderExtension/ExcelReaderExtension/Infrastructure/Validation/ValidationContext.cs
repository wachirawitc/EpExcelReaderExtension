using ExcelReaderExtension.Infrastructure.Interface;
using ExcelReaderExtension.Infrastructure.Model;
using System;
using System.Linq.Expressions;

namespace ExcelReaderExtension.Infrastructure.Validation
{
    public class ValidationContext
    {
        public IValidationRule Rule { get; set; }

        public Expression<Func<Cell, string>> Message { get; set; }
    }
}