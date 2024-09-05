using ENSEK.Models;
using System;
using System.Globalization;
using System.Text.RegularExpressions;



namespace ENSEK.Services
{

    public class MeterReadingValidator : IMeterReadingValidator
    {
        public bool Validate(MeterReadingUploadModel reading, out MeterReadingModel meterReadingModel)
        {
            meterReadingModel = null;
            string dateString = reading.MeterReadingDateTime;
            string format = "dd/MM/yyyy HH:mm";
            DateTime parsedDateTime;

            // 1. Validate AccountId must be exactly 4 digits (numeric only)
            if (!Regex.IsMatch(reading.AccountId.ToString(), @"^\d{4}$"))
                return false;

            // Check if the string can be parsed into the expected DateTime format
            if (!DateTime.TryParseExact(
                dateString,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedDateTime))
            {
                return false;
            }

            // 3. Validate MeterReadValue length must be <= 9 digits and must be a number
            if (reading.MeterReadValue.ToString().Length > 9 || !Regex.IsMatch(reading.MeterReadValue.ToString(), @"^\d+$"))
                return false;

            meterReadingModel = new MeterReadingModel();
            meterReadingModel.AccountId = Convert.ToInt32(reading.AccountId);
            meterReadingModel.MeterReadValue = Convert.ToInt32(reading.MeterReadValue);
            meterReadingModel.MeterReadingDateTime = parsedDateTime;
            // If all validations pass, return true
            return true;
        }
    }
}
