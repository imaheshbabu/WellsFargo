using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WellsFargo.OMS.Library.Loggers;
using WellsFargo.OMS.Library.Models;

namespace WellsFargo.OMS.Library.Processors
{
    public class AAAProcessor : IAAAProcessor
    {
        private readonly ILogger _logger;
        private readonly Configuration _configuration;
        public AAAProcessor(ILogger logger, Configuration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
        }
        public void writeToFile(IEnumerable<TransactionDetails> transactionDetails)
        {
            var aaaTransactiojns = transactionDetails.Where(order => order.transaction.OMS == "AAA");
            if (aaaTransactiojns.Count() == 0)
            {
                _logger.LogError($"There are no records to process AAA transactions");
                return;
            }

            var filePath = Path.Combine(_configuration.OutputPath, $"aaa-orders.aaa");
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"ISIN,PortfolioCode,Nominal,TransactionType");

                foreach (var aaaOrder in aaaTransactiojns)
                {
                    if (string.IsNullOrEmpty(aaaOrder.security?.ISIN) || string.IsNullOrEmpty(aaaOrder.portfolio?.PortfolioCode))
                    {
                        _logger.LogError($"Order with empty ISIN or PortfolioCode: {aaaOrder}");
                    }
                    else
                    {
                        try
                        {

                            writer.WriteLine($"{aaaOrder.security.ISIN},{aaaOrder.portfolio.PortfolioCode},{aaaOrder.transaction.Nominal},{aaaOrder.transaction.TransactionType}");

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
