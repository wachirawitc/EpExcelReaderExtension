namespace ExcelReaderExtension.Infrastructure.Interface
{
    public interface IParse<out T>
    {
        T Get();
    }
}