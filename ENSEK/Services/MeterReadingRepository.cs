using ENSEK.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;


namespace ENSEK.Services
{

    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly string _connectionString;

        public MeterReadingRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task BulkInsertMeterReadingsAsyncoldd(List<MeterReadingModel> meterReadings)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            //await connection.OpenAsync();

            using var bulkCopy = new SqlBulkCopy(connection)
            {
                DestinationTableName = "MeterReadings"
            };

            var dataTable = new DataTable();
            dataTable.Columns.Add("AccountId", typeof(int));
            dataTable.Columns.Add("MeterReadingDateTime", typeof(DateTime));
            dataTable.Columns.Add("MeterReadValue", typeof(int));

            foreach (var reading in meterReadings)
            {
                dataTable.Rows.Add(reading.AccountId, reading.MeterReadingDateTime, reading.MeterReadValue);
            }

            await bulkCopy.WriteToServerAsync(dataTable);
        }


        public async Task BulkInsertMeterReadingsAsync(List<MeterReadingModel> meterReadings)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var bulkCopy = new SqlBulkCopy(connection)
            {
                DestinationTableName = "dbo.MeterReadings" // Ensure the table name includes schema
            };

            // Create a DataTable with the correct column names and types
            var dataTable = new DataTable();
            dataTable.Columns.Add("AccountId", typeof(int));                  // Should match SQL column name
            dataTable.Columns.Add("MeterReadingDateTime", typeof(DateTime));  // Should match SQL column name
            dataTable.Columns.Add("MeterReadValue", typeof(int));              // Should match SQL column name

            // Populate DataTable
            foreach (var reading in meterReadings)
            {
                dataTable.Rows.Add(reading.AccountId, reading.MeterReadingDateTime, reading.MeterReadValue);
            }

            // Map columns to ensure correct data mapping
            bulkCopy.ColumnMappings.Add("AccountId", "AccountId");
            bulkCopy.ColumnMappings.Add("MeterReadingDateTime", "MeterReadingDateTime");
            bulkCopy.ColumnMappings.Add("MeterReadValue", "MeterReadValue");

            // Perform the bulk copy
            await bulkCopy.WriteToServerAsync(dataTable);
        }



        public async Task BulkInsertMeterReadingsAsyncold(List<MeterReadingModel> meterReadings)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();  // Use async open

            using var bulkCopy = new SqlBulkCopy(connection)
            {
                DestinationTableName = "dbo.MeterReadings"  // Include schema name if necessary
            };

            var dataTable = new DataTable();
            dataTable.Columns.Add("AccountId", typeof(int));
            dataTable.Columns.Add("MeterReadingDateTime", typeof(DateTime));
            dataTable.Columns.Add("MeterReadValue", typeof(int));

            foreach (var reading in meterReadings)
            {
                // Ensure MeterReadingDateTime is in correct format
                if (DateTime.TryParseExact(
                    reading.MeterReadingDateTime.ToString(), // Assuming reading.MeterReadingDateTime is DateTime
                    "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsedDateTime))
                {
                    dataTable.Rows.Add(reading.AccountId, parsedDateTime, reading.MeterReadValue);
                }
                else
                {
                    // Handle invalid date format if needed
                    Console.WriteLine($"Invalid date format: {reading.MeterReadingDateTime}");
                }
            }

            await bulkCopy.WriteToServerAsync(dataTable);
        }


    }

}
