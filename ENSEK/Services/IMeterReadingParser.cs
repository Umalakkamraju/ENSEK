

using ENSEK.Models;

namespace ENSEK.Services
{
    public interface IMeterReadingParser
    {
        List<MeterReadingUploadModel> ParseCsv(IFormFile file);
    }

}
