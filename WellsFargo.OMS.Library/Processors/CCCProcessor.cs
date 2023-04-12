using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WellsFargo.OMS.Library.Loggers;
using WellsFargo.OMS.Library.Models;

namespace WellsFargo.OMS.Library.Processors
{
    public class CCCProcessor: ICCCProcessor
    {
        private readonly ILogger _logger;
        private readonly Configuration _configuration;
        public CCCProcessor(ILogger logger, Configuration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
        }

        public void writeToFile(IEnumerable<TransactionDetails> transactionDetails)
        {
            var cssTransactions = transactionDetails.Where(order => order.transaction.OMS == "CCC");
            if (cssTransactions.Count() == 0)
            {
                _logger.LogError($"There are no records to process CCC transactions");
                return;
            }

            var filePath = Path.Combine(_configuration.OutputPath, $"ccc_orders.ccc");
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var cccOrder in cssTransactions)
                {
                    if (string.IsNullOrEmpty(cccOrder.security?.Ticker) || string.IsNullOrEmpty(cccOrder.portfolio?.PortfolioCode))
                    {
                        _logger.LogError($"Order with empty Ticker or PortfolioCode: {cccOrder}");
                    }
                    else
                    {
                        try
                        {

                            writer.WriteLine($"{cccOrder.portfolio.PortfolioCode},{cccOrder.security.Ticker},{cccOrder.transaction.Nominal},{cccOrder.transaction.TransactionType}");

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