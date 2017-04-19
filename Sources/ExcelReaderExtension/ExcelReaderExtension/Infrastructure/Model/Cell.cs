namespace ExcelReaderExtension.Infrastructure.Model
{
    public class Cell
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public string WorksheetName { get; set; }

        /// <summary>
        /// Examples of addresses are "A1" "B1:C2" "A:A" "1:1" "A1:E2,G3:G5"
        /// </summary>
        public string Address { get; set; }
    }
}