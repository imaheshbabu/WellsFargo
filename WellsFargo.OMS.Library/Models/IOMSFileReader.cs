using System.Collections.Generic;

namespace WellsFargo.OMS.Library.Models
{
    public interface IOMSFileReader<T>
    {
        IEnumerable<T> ReadFile(string pathToRead);
    }
}
