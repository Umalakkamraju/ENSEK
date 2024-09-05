

using ENSEK.Models;

namespace ENSEK.Services
{
    public interface IMeterReadingRepository
    {
        Task BulkInsertMeterReadingsAsync(List<MeterReadingModel> meterReadings);
    }
}
