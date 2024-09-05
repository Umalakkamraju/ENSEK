

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ENSEK.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace ENSEK.Services
{
    public class MeterReadingParser : IMeterReadingParser
    {
        public List<MeterReadingUploadModel> ParseCsv(IFormFile file)
        {
            var meterReadings = new List<MeterReadingUploadModel>();

            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                BadDataFound = context => { /* Handle bad data */ }
            }))
            {
                var records = csv.GetRecords<MeterReadingUploadModel>();
                foreach (var record in records)
                {
                    meterReadings.Add(new MeterReadingUploadModel
                    {
                        AccountId = record.AccountId,
                        MeterReadingDateTime = record.MeterReadingDateTime,
                        MeterReadValue = record.MeterReadValue
                    });
                }
            }

            return meterReadings;
        }
    }
}
