using MongoDB.Bson.Serialization.Attributes;

namespace front_back.Database
{
    public class Account
    {


        [BsonId]
        public int Id { get; set; }

        public string BankName { get; set; } = string.Empty;
        public double Balance { get; set; }
       
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
