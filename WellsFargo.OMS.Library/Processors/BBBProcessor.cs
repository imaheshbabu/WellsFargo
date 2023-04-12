using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WellsFargo.OMS.Library.Loggers;
using WellsFargo.OMS.Library.Models;

namespace WellsFargo.OMS.Library.Processors
{
    public class BBBProcessor: IBBBProcessor
    {
        private readonly ILogger _logger;
        private readonly Configuration _configuration;
        public BBBProcessor(ILogger logger, Configuration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
        }

        public void writeToFile(IEnumerable<TransactionDetails> transactionDetails)
        {
            var bbbTransactions = transactionDetails.Where(order => order.transaction.OMS == "BBB");
            if (bbbTransactions.Count() == 0)
            {
                _logger.LogError($"There are no records to process BBB transactions");
                return;
            }

            var filePath = Path.Combine(_configuration.OutputPath, $"bbb_orders.bbb");
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Cusip,PortfolioCode,Nominal,TransactionType");

                foreach (var bbbOrder in bbbTransactions)
                {
                    if (string.IsNullOrEmpty(bbbOrder.security?.CUSIP) || string.IsNullOrEmpty(bbbOrder.portfolio?.PortfolioCode))
                    {
                        _logger.LogError($"Order with empty Cusip or PortfolioCode: {bbbOrder}");
                    }
                    else
                    {
                        try
                        {

                            writer.WriteLine($"{bbbOrder.security.CUSIP}|{bbbOrder.portfolio.PortfolioCode}|{bbbOrder.transaction.Nominal}|{bbbOrder.transaction.TransactionType}");

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error writing file {filePath}: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}
