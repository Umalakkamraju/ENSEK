

namespace ENSEK.Models
{

    //for value based equality; immutable
    public record MeterReadingModel
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int MeterReadValue { get; set; }
    }


}
