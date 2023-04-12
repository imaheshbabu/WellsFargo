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
            /* return from t in transactions
                    join s in securities on t.SecurityId equals s.SecurityId into securityJoin
                    from s in securityJoin.DefaultIfEmpty()
                    join p in portfolios on t.PortfolioId equals p.PortfolioId into portfolioJoin
                    from p in portfolioJoin.DefaultIfEmpty()
                    join c in securities on s.CUSIP equals c.CUSIP into cusipJoin
                    from c in cusipJoin.DefaultIfEmpty()
                    select new TransactionDetails
                    {
                        transaction = t,
                        security = s,
                        portfolio = p
                    };*/
        }
    }
}
