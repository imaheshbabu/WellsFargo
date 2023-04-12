using System.Collections.Generic;

namespace WellsFargo.OMS.Library.Models
{
    public interface ISecuritiesFileReader : IOMSFileReader<Security>
    {
        new IEnumerable<Security> ReadFile(string pathToRead);
    }
}
