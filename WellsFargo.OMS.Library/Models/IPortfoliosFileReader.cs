using System.Collections.Generic;

namespace WellsFargo.OMS.Library.Models
{
    public interface IPortfoliosFileReader: IOMSFileReader<Portfolio>
    {
        new IEnumerable<Portfolio> ReadFile(string pathToRead);
    }
}
