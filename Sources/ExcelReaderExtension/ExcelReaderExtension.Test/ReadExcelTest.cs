using ExcelReaderExtension.Extensions;
using ExcelReaderExtension.Infrastructure.Converter;
using NUnit.Framework;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelReaderExtension.Test
{
    [TestFixture]
    public class ReadExcelTest
    {
        [Test]
        public void Test_Valid_ReadInt()
        {
            const int expected = 1;
            const string filePath = @"\DataSources\Test_Valid_ReadInt.xlsx";

            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            var fileInfo = new FileInfo(projectPath + filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[1];

                var value = worksheet.Cells[1, 1]
                    .Cast<int>()
                    .NumericOnly()
                        .WithMessage(resource => $"{resource.Address} is support numeric only")
                    .Get();

                Assert.AreEqual(expected, value);
            }
        }

        [Test]
        public void Test_Valid_ReadNullableDateTime()
        {
            const string filePath = @"\DataSources\Test_Valid_ReadNullableDateTime.xlsx";

            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            var fileInfo = new FileInfo(projectPath + filePath);

            DateTime? expected = new DateTime(2016, 4, 19);

            var supportDateFormat = new List<string> { "dd/MM/yyyy" };
            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[1];

                var actual = worksheet.Cells[1, 1]
                    .Cast<DateTime?>(value => new NullableDateTimeConverter(value, supportDateFormat))
                    .Get();

                Assert.AreEqual(expected, actual);
            }
        }
    }
}