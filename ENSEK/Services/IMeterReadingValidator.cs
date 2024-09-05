

using ENSEK.Models;

namespace ENSEK.Services
{
    public interface IMeterReadingValidator
    {
        bool Validate(MeterReadingUploadModel reading,out MeterReadingModel meterReadingModel);
    }
}
