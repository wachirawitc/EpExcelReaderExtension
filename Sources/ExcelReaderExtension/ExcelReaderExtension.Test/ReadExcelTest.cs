using ExcelReaderExtension.Extensions;
using NUnit.Framework;
using OfficeOpenXml;
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
            const string filePath = @"E:\Source\ExcelReaderExtension\Sources\ExcelReaderExtension\ExcelReaderExtension.Test\DataSources\Examle1.xlsx";
            var fileInfo = new FileInfo(filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[1];

                var value = worksheet.Cells[1, 1]
                    .Cast<int>()
                    .NumericOnly()
                        .WithMessage(cell => $"{cell.Address} is support numeric only")
                    .Get();

                Assert.AreEqual(expected, value);
            }
        }
    }
}