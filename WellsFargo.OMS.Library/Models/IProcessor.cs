using System.Collections.Generic;

namespace WellsFargo.OMS.Library.Models
{
    public interface IProcessor
    {
        void writeToFile(IEnumerable<TransactionDetails> transactionDetails);
    }
}
