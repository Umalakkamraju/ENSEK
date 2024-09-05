1. Run the following script to crerate a database, database tables and to insert test data. This script is rerunnable


                    USE MASTER
                    go
                    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'MeterReadingsDb')
                    BEGIN
  
  
                      CREATE DATABASE MeterReadingsDb;

                    END
                    GO

                    USE MeterReadingsDb;
                    GO

                    -- I am dropping tables if already exists 

                    IF OBJECT_ID('dbo.MeterReadings', 'U') IS NOT NULL
                    BEGIN

                        DROP TABLE dbo.MeterReadings;
                    END
                    GO


                    IF OBJECT_ID('dbo.TestAccounts', 'U') IS NOT NULL
                    BEGIN
                        DROP TABLE dbo.TestAccounts;
                    END
                    GO


                    -- I am creating tables agan
                    CREATE TABLE dbo.TestAccounts (
                        AccountId INT PRIMARY KEY,
                        FirstName NVARCHAR(100),
                        LastName NVARCHAR(100)
                    );
                    GO

                    -- Create the MeterReadings table
                    CREATE TABLE dbo.MeterReadings (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        AccountId INT,
                        MeterReadingDateTime DATETIME,
                        MeterReadValue INT,
                        FOREIGN KEY (AccountId) REFERENCES dbo.TestAccounts(AccountId)
                    );
                    GO


                    -- Load test accounts data:
                    INSERT INTO TestAccounts (AccountId, FirstName, LastName)
                    VALUES
                        (1234, 'Freya', 'Test'),
                        (1239, 'Noddy', 'Test'),
                        (1240, 'Archie', 'Test'),
                        (1241, 'Lara', 'Test'),
                        (1242, 'Tim', 'Test'),
                        (1243, 'Graham', 'Test'),
                        (1244, 'Tony', 'Test'),
                        (1245, 'Neville', 'Test'),
                        (1246, 'Jo', 'Test'),
                        (1247, 'Jim', 'Test'),
                        (1248, 'Pam', 'Test'),
                        (2233, 'Barry', 'Test'),
                        (2344, 'Tommy', 'Test'),
                        (2345, 'Jerry', 'Test'),
                        (2346, 'Ollie', 'Test'),
                        (2347, 'Tara', 'Test'),
                        (2348, 'Tammy', 'Test'),
                        (2349, 'Simon', 'Test'),
                        (2350, 'Colin', 'Test'),
                        (2351, 'Gladys', 'Test'),
                        (2352, 'Greg', 'Test'),
                        (2353, 'Tony', 'Test'),
                        (2355, 'Arthur', 'Test'),
                        (2356, 'Craig', 'Test'),
                        (4534, 'JOSH', 'TEST'),
                        (6776, 'Laura', 'Test'),
                        (8766, 'Sally', 'Test');
                    GO

2. Change connection string in appsettings.json
3. Launch the solution using VisualStudio 2022 and 
4. Start debugging, that will launch Swagger UI
5. Tryout the post method uploading meter readings CSV file


