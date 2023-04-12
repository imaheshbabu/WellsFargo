using System;
using System.Collections.Generic;
using System.IO;
using WellsFargo.OMS.Library.Loggers;
using WellsFargo.OMS.Library.Models;

namespace WellsFargo.OMS.Library.Readers
{
    public class SecuritiesFileReader: ISecuritiesFileReader
    {
        private readonly ILogger _logger;
        public SecuritiesFileReader(ILogger logger)
        {
            this._logger = logger;
        }

        public IEnumerable<Security> ReadFile(string pathToRead)
        {
            var securities = new List<Security>();

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

                        var security = new Security
                        {
                            SecurityId = int.Parse(values[0]),
                            ISIN = values[1],
                            Ticker = values[2],
                            CUSIP = values[3],
                        };

                        securities.Add(security);
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error reading securities file: {ex.Message}");
            }

            return securities;
        }
    }
}
