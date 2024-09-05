using ENSEK.Models;
using ENSEK.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace ENSEK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterReadingRepository _repository;
        private readonly IMeterReadingValidator _validator;
        private readonly IMeterReadingParser _parser;
        private readonly ILogger<MeterReadingController> _logger;

        public MeterReadingController(IMeterReadingRepository repository, IMeterReadingValidator validator, IMeterReadingParser parser, ILogger<MeterReadingController> logger)
        {
            _repository = repository;
            _validator = validator;
            _parser = parser;
            _logger = logger;
        }

        [HttpPost("meter-reading-uploads")]
        public async Task<IActionResult> UploadMeterReadings(IFormFile file)
        {
            try
            {


                // This is to Log the start of the upload process
                _logger.LogInformation("Starting meter upload for file: {FileName}", file.FileName);

                // Parsing the CSV file to extract meter readings
                var meterReadings = _parser.ParseCsv(file);


                // This is to create a ConcurrentBag to store valid readings
                var validReadings = new ConcurrentBag<MeterReadingModel>();

                // Using parallel processing to validate each reading
                Parallel.ForEach(meterReadings, reading =>
                {
                    try
                    {
                        MeterReadingModel meterReadingModel = null;
                        if (_validator.Validate(reading, out meterReadingModel))
                        {
                            validReadings.Add(meterReadingModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error for individual reading validation failures
                        _logger.LogError(ex, "Error validating readings for AccountId: {AccountId}", reading.AccountId);
                    }
                });


                // This is to log the number of valid readings before insertings
                _logger.LogInformation("Inserting {ValidReadingsCount} valid readings into the database", validReadings.Count);

                // Perform bulk insertion of valid readings. This to increase throughput
                await _repository.BulkInsertMeterReadingsAsync(validReadings.ToList());



                // Create result model
                var result = new MeterReadingResultModel
                {
                    Successful = validReadings.Count,
                    Failed = meterReadings.Count - validReadings.Count
                };



                // Log the result
                _logger.LogInformation("Upload completed.: {Successful}, Failed: {Failed}", result.Successful, result.Failed);

                // Return the result as an HTTP response
                return Ok(result);
            }
            catch (Exception ex)
            {

                // Log any errors that occurred during the uploading process
                _logger.LogError(ex, "An error occurred during the meter reading upload process");


                // Returns an internal server error response
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the meter readings.");
            }
        }


    }
}


