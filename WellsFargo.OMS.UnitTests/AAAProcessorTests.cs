using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using WellsFargo.OMS.Library.Loggers;
using WellsFargo.OMS.Library.Models;
using WellsFargo.OMS.Library.Processors;
using WellsFargo.OMS.Library.Utils;

namespace WellsFargo.OMS.UnitTests
{
    [TestClass]
    public class AAAProcessorTests
    {
        private IEnumerable<TransactionDetails> orders;
        private Mock<ILogger> loggerMock;
        private IAAAProcessor aaaProcessor;
        private string _outputDirectory;
        private IEnumerable<TransactionDetails> transactionDetails;

        [TestInitialize]
        public void TestInitialize()
        {
            _outputDirectory = Path.Combine(Path.GetTempPath(), "OMSProcessorTests");
            if (!Directory.Exists(_outputDirectory))
            {
                Directory.CreateDirectory(_outputDirectory);
            }

            loggerMock = new Mock<ILogger>();
            aaaProcessor = new AAAProcessor(loggerMock.Object, new Configuration(_outputDirectory));
         
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Clean up test data
            orders = null;

            // Delete test output files
            if (Directory.Exists(_outputDirectory))
            {
                Directory.Delete(_outputDirectory, true);
            }
        }

        [TestMethod]
        public void Process_EmptyOrderList_LogsError()
        {
            var emptyOrders = new List<TransactionDetails>();

            aaaProcessor.writeToFile(emptyOrders);

            loggerMock.Verify(x => x.LogError(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Process_OrderWithEmptyISIN_LogsError()
        {
            var transactionDetails = new List<TransactionDetails>
            {
                new TransactionDetails
                {
                    security = new Security(),
                    portfolio = new Portfolio { PortfolioCode = "PortfolioCode1" },
                    transaction = new Transaction { Nominal = 100, TransactionType = "Type1" }
                }
            };

            // Act
            aaaProcessor.writeToFile(transactionDetails);

            // Assert
            loggerMock.Verify(x => x.LogError(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Process_OrderWithEmptyPortfolioCode_LogsError()
        {
            var transactionDetails = new List<TransactionDetails>
            {
                new TransactionDetails
                {
                    security = new Security { ISIN = "ISIN1" },
                    portfolio = new Portfolio(),
                    transaction = new Transaction { Nominal = 100, TransactionType = "Type1" }
                }
            };


            // Act
            aaaProcessor.writeToFile(transactionDetails);

            // Assert
            loggerMock.Verify(x => x.LogError(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Process_NonEmptyOrderList_WritesToFile()
        {
            // Create test data
            var portfolios = new List<Portfolio>
            {
                new Portfolio { PortfolioId = 1, PortfolioCode = "p1" },
                new Portfolio { PortfolioId = 2, PortfolioCode = "p2" }
            };

            var securities = new List<Security>
            {
                new Security { SecurityId = 1, CUSIP = "CUSIP0001", ISIN = "ISIN11111111", Ticker = "s1" },
                new Security { SecurityId = 2, CUSIP = "CUSIP0002", ISIN = "ISIN22222222", Ticker = "s2" }
            };

            var transactions = new List<Transaction>
            {
                new Transaction {  OMS = "AAA", SecurityId = 1, PortfolioId = 1, Nominal = 10, TransactionType = "BUY" },
                new Transaction { OMS = "BBB", SecurityId = 2, PortfolioId = 2, Nominal = 20, TransactionType = "SELL" },
                new Transaction {  OMS = "CCC", SecurityId = 1, PortfolioId = 2, Nominal = 30, TransactionType = "BUY" }
            };
            transactionDetails = TransactionUtils.createTransactionDetails(transactions, portfolios, securities);

            var expectedFilePath = Path.Combine(_outputDirectory, "aaa-orders.aaa");


            // Act
            aaaProcessor.writeToFile(transactionDetails);

            // Assert
            loggerMock.Verify(x => x.LogError(It.IsAny<string>()), Times.Never);
            Assert.IsTrue(File.Exists(expectedFilePath));
            var fileContents = File.ReadAllLines(expectedFilePath);
            Assert.AreEqual(2, fileContents.Length);
            Assert.AreEqual("ISIN,PortfolioCode,Nominal,TransactionType", fileContents[0]);
            Assert.AreEqual("ISIN11111111,p1,10,BUY", fileContents[1]);
        }


    }

}
