```csharp
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
```
