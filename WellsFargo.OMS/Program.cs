using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Linq;
using WellsFargo.OMS.Library.Loggers;
using WellsFargo.OMS.Library.Models;
using WellsFargo.OMS.Library.Processors;
using WellsFargo.OMS.Library.Readers;
using WellsFargo.OMS.Library.Utils;
using Configuration = WellsFargo.OMS.Library.Models.Configuration;

namespace WellsFargo.OMS
{
    class Program
    {
        private static readonly string transactionsFilePath = ConfigurationManager.AppSettings["TransactionsFilePath"];
        private static readonly string securitiesFilePath = ConfigurationManager.AppSettings["SecuritiesFilePath"];
        private static readonly string portfoliosFilePath = ConfigurationManager.AppSettings["PortfoliosFilePath"];
        private static readonly string outputPath = ConfigurationManager.AppSettings["OutputPath"];

        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddSingleton<ILogger, ConsoleLogger>()
            .AddSingleton(new Configuration(outputPath))
                .AddSingleton<IPortfoliosFileReader, PortfoliosFileReader>()
                .AddSingleton<ISecuritiesFileReader, SecuritiesFileReader>()
                .AddSingleton<ITransactionsFileReader, TransactionsFileReader>()
                .AddSingleton<IAAAProcessor, AAAProcessor>()
                .AddSingleton<IBBBProcessor, BBBProcessor>()
                .AddSingleton<ICCCProcessor, CCCProcessor>()
            .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger>();
            var portfoliosFileReader = serviceProvider.GetService<IPortfoliosFileReader>();
            var securitiesFileReader = serviceProvider.GetService<ISecuritiesFileReader>();
            var transactionsFileReader = serviceProvider.GetService<ITransactionsFileReader>();
            var aaaProcessor = serviceProvider.GetService<IAAAProcessor>();
            var bbbProcessor = serviceProvider.GetService<IBBBProcessor>();
            var cccProcessor = serviceProvider.GetService<ICCCProcessor>();


            var transactions = transactionsFileReader.ReadFile(transactionsFilePath);
            var securities = securitiesFileReader.ReadFile(securitiesFilePath);
            var portfolios = portfoliosFileReader.ReadFile(portfoliosFilePath);
            var transactionDetails = TransactionUtils.createTransactionDetails(transactions, portfolios, securities);

            aaaProcessor.writeToFile(transactionDetails);
            bbbProcessor.writeToFile(transactionDetails);
            cccProcessor.writeToFile(transactionDetails);

            logger.LogInformation("Processing complete. please check for any errors. Press any key to exit....");
            Console.ReadKey();
        }
    }
}