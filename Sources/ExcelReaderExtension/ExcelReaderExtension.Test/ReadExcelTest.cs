using ExcelReaderExtension.Extensions;
using NUnit.Framework;
using OfficeOpenXml;
using System;
using System.IO;

namespace ExcelReaderExtension.Test
{
    [TestFixture]
    public class ReadExcelTest
    {
        [Test]
        public void Test_ReadInt()
        {
            const int expected = 1;
            const string filePath = @"\DataSources\Examle1.xlsx";

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
    }
}