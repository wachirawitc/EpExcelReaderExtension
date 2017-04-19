Supporter extension for EPPlus to read and parse value in cell with condition.

**Basic**
For example value in row 1 and column 1 is "1"
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


**Basic parse value**
For example value in row 1 and column 1 is "21/04/2016"
```csharp
            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[1];

                DateTime? value = worksheet.Cells[1, 1]
                            .Cast(value => ToDateTime(value))
                            .NotNull()
                            .Get();

            }
            
            private static DateTime? ToDateTime(object value)
            {
                        string[] formats = { "dd/MM/yyyy" };
                        DateTime outTime;
                        var isSuccess = DateTime.TryParseExact(
                                    value.ToString(), 
                                    formats, 
                                    CultureInfo.InvariantCulture, 
                                    DateTimeStyles.None, 
                                    out outTime);
                        return isSuccess == false ? (DateTime?)null : outTime;
            }
            
```
