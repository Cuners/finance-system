namespace Finance.Domain
{
    public partial class Account
    {
        public int AccountId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Balance { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
