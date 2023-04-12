using System;
using System.Collections.Generic;
using System.IO;
using WellsFargo.OMS.Library.Loggers;
using WellsFargo.OMS.Library.Models;

namespace WellsFargo.OMS.Library.Readers
{
    public class TransactionsFileReader : ITransactionsFileReader
    {
        private readonly ILogger _logger;
        public TransactionsFileReader(ILogger logger)
        {

            this._logger = logger;
        }

        public IEnumerable<Transaction> ReadFile(string pathToRead)
        {
            var transactions = new List<Transaction>();

            try
            {
                using (var reader = new StreamReader(pathToRead))
                {
                    // skip header
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        var transaction = new Transaction
                        {
                            SecurityId = int.Parse(values[0]),
                            PortfolioId = int.Parse(values[1]),
                            Nominal = decimal.Parse(values[2]),
                            OMS = values[3],
                            TransactionType = values[4]
                        };

                        transactions.Add(transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error reading transactions file: {ex.Message}");
            }

            return transactions;
        }
    }
}