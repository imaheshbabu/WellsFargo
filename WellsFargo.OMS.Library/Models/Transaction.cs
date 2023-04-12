namespace WellsFargo.OMS.Library.Models
{
    public class Transaction
    {
        public int SecurityId { get; set; }
        public int PortfolioId { get; set; }
        public decimal Nominal { get; set; }
        public string OMS { get; set; }
        public string TransactionType { get; set; }
    }
}
