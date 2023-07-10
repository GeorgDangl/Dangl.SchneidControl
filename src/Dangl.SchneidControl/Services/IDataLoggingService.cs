namespace Dangl.SchneidControl.Services
{
    public interface IDataLoggingService
    {
        Task ReadAndSaveValuesAsync();
    }
}
