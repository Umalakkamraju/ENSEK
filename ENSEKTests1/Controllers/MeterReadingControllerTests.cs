using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENSEK.Controllers;
using ENSEK.Models;
using ENSEK.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;



namespace ENSEK.Controllers.Tests
{
    [TestClass()]
    public class MeterReadingControllerTests
    {
        private MeterReadingController _controller;
        private Mock<IMeterReadingRepository> _mockRepository;
        private Mock<IMeterReadingValidator> _mockValidator;
        private Mock<IMeterReadingParser> _mockParser;
         Mock<ILogger<MeterReadingController>> _logger;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IMeterReadingRepository>();
            _mockValidator = new Mock<IMeterReadingValidator>();
            _mockParser = new Mock<IMeterReadingParser>();
            _logger= new Mock<ILogger<MeterReadingController>>();

            _controller = new MeterReadingController(
                _mockRepository.Object,
                _mockValidator.Object,
                _mockParser.Object,
                _logger.Object
            );
        }

        [TestMethod()]
        public async Task UploadMeterReadings_ValidData_ReturnsSuccess()
        {
            // Arrange
            var file = new FormFile(new MemoryStream(), 0, 0, "Data", "Meter_reading.csv");

            var meterReadings = new List<MeterReadingUploadModel>
            {
                new MeterReadingUploadModel { AccountId = "1234", MeterReadingDateTime = DateTime.Now.ToString(), MeterReadValue = "1000" },
                new MeterReadingUploadModel { AccountId = "5678", MeterReadingDateTime = DateTime.Now.ToString(), MeterReadValue = "2000" }
            };

            MeterReadingModel meterReadingModel;


            _mockParser.Setup(p => p.ParseCsv(It.IsAny<IFormFile>())).Returns(meterReadings);
            _mockValidator.Setup(v => v.Validate(It.IsAny<MeterReadingUploadModel>(), out meterReadingModel)).Returns(true);
            _mockRepository.Setup(r => r.BulkInsertMeterReadingsAsync(It.IsAny<List<MeterReadingModel>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UploadMeterReadings(file) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            var resultModel = result.Value as MeterReadingResultModel;
            Assert.IsNotNull(resultModel);



            Assert.AreEqual(meterReadings.Count, resultModel.Successful);
            Assert.AreEqual(0, resultModel.Failed);
        }

        [TestMethod()]
        public async Task UploadMeterReadings_InvalidData_ReturnsSuccessWithFailures()
        {
            // Arrange
            var file = new FormFile(new MemoryStream(), 0, 0, "Data", "Meter_reading.csv");

            var meterReadings = new List<MeterReadingUploadModel>
            {
                new MeterReadingUploadModel { AccountId = "1234", MeterReadingDateTime = DateTime.Now.ToString(), MeterReadValue = "1000" },
            
                new MeterReadingUploadModel { AccountId = "5678", MeterReadingDateTime = DateTime.Now.ToString(), MeterReadValue = "2000" }
            };
            
            MeterReadingModel meterReadingModel;
            _mockParser.Setup(p => p.ParseCsv(It.IsAny<IFormFile>())).Returns(meterReadings);
            _mockValidator.Setup(v => v.Validate(It.IsAny<MeterReadingUploadModel>(), out meterReadingModel)).Returns(false);
            
            
            
            
            _mockRepository.Setup(r => r.BulkInsertMeterReadingsAsync(It.IsAny<List<MeterReadingModel>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UploadMeterReadings(file) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            var resultModel = result.Value as MeterReadingResultModel;
            Assert.IsNotNull(resultModel);
            
            
            
            Assert.AreEqual(0, resultModel.Successful);
            Assert.AreEqual(meterReadings.Count, resultModel.Failed);
        }
    }
}
