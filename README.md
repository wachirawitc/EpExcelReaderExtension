Supporter extension for EPPlus to read and parse value in cell with condition.

**Basic**
```csharp
            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[1];

                int value = worksheet.Cells[1, 1]
                    .Cast<int>()
                    .NumericOnly()
                        .WithMessage(cell => $"{cell.Address} is support numeric only")
                    .Get();

            }
```
