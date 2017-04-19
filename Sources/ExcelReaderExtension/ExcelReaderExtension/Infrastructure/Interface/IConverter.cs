namespace ExcelReaderExtension.Infrastructure.Interface
{
    public interface IConverter<out T>
    {
        T Get();
    }
}