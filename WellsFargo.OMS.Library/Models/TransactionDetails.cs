namespace WellsFargo.OMS.Library.Models
{
    public class TransactionDetails
    {
        public Transaction transaction { get; set; }

        public Security security { get; set; }

        public Portfolio portfolio { get; set; }
    }
}
