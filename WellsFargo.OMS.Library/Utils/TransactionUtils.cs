using System.Collections.Generic;
using System.Linq;
using WellsFargo.OMS.Library.Models;

namespace WellsFargo.OMS.Library.Utils
{
    public sealed class TransactionUtils
    {
        public static IEnumerable<TransactionDetails> createTransactionDetails(IEnumerable<Transaction> transactions, IEnumerable<Portfolio> portfolios, IEnumerable<Security> securities)
        {
            return from transaction in transactions
                   join portfolio in portfolios on transaction.PortfolioId equals portfolio.PortfolioId into portfolioGroup
                   from portfolio in portfolioGroup.DefaultIfEmpty()
                   join security in securities on transaction.SecurityId equals security.SecurityId into securityGroup
                   from security in securityGroup.DefaultIfEmpty()
                   select new TransactionDetails
                   {
                       transaction = transaction,
                       security = security,
                       portfolio = portfolio
                   };
        }
    }
}
