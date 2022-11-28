using MongoDB.Bson.Serialization.Attributes;

namespace front_back.Database
{
    public class Client
    {

        [BsonId]
        public int Id { get; set; } = 0;


        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
