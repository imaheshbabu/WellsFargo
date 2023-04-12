using System.Collections.Generic;

namespace WellsFargo.OMS.Library.Models
{
    public interface ITransactionsFileReader : IOMSFileReader<Transaction>
    {
        new IEnumerable<Transaction> ReadFile(string pathToRead);
    }
}
