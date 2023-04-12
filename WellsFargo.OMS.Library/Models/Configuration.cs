namespace WellsFargo.OMS.Library.Models
{
    public class Configuration
    {
        private string outputPath;
        public Configuration(string _outputPath)
        {
            this.outputPath = _outputPath;
        }
        public string OutputPath { get => this.outputPath;  }
    }
}
