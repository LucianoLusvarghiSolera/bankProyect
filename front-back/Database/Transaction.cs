using MongoDB.Bson.Serialization.Attributes;

namespace front_back.Database
{
    public class Transaction
    {

        [BsonId]
        public int Id { get; set; }

        public string RecipientsName { get; set; } = string.Empty;

        public string Concept { get; set; } = string.Empty;

        public double Amount { get; set; }


    }
}
