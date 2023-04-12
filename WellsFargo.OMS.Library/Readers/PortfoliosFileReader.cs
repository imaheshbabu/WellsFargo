using System;
using System.Collections.Generic;
using System.IO;
using WellsFargo.OMS.Library.Loggers;
using WellsFargo.OMS.Library.Models;

namespace WellsFargo.OMS.Library.Readers
{

    public class PortfoliosFileReader : IPortfoliosFileReader
    {
        private readonly ILogger _logger;
        public PortfoliosFileReader(ILogger logger)
        {
            this._logger = logger;
        }

        public IEnumerable<Portfolio> ReadFile(string pathToRead)
        {
            var portfolios = new List<Portfolio>();

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

                        var portfolio = new Portfolio
                        {
                            PortfolioId = int.Parse(values[0]),
                            PortfolioCode = values[1],
                        };

                        portfolios.Add(portfolio);
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error reading portfolios file: {ex.Message}");
            }

            return portfolios;
        }
    }
}
