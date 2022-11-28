namespace front_back.Database
{
    public class BankDatabaseSettings
    {
        
        public string ConnectionString { get; set; } = "mongodb+srv://admin:admin@cluster0.chqxutx.mongodb.net/?retryWrites=true&w=majority";

        public string DatabaseName { get; set; } = "Team2-Bank-DB";

        public string BankCollectionName { get; set; } = "Bank";
    }
}
