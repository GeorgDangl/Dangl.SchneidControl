using Dangl.SchneidControl.Models.Services;

namespace Dangl.SchneidControl.Services
{
    public interface IDataLoggingService
    {
        Task<ValuesResult> ReadAndSaveValuesAsync();
    }
}
