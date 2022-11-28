using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace front_back.Database
{
    public class DB_controller
    {
        private static DB_controller safe_instance = null;

       public  static DB_controller instance() { 
            
            if(safe_instance == null)
            {
                safe_instance = new DB_controller();
                safe_instance.DB_connect();
            }

            return safe_instance;
        }

        private DB_controller() { }

        private IMongoCollection<Client> _bank_DB = null;

        public bool DB_connect()
        {
            if(_bank_DB != null) return true;

            BankDatabaseSettings dabaseSettings = new BankDatabaseSettings();

            var mongoClient = new MongoClient(dabaseSettings.ConnectionString);
            if(mongoClient == null)
            {
                return false;
            }

            _bank_DB = mongoClient.GetDatabase(dabaseSettings.DatabaseName).GetCollection<Client>(dabaseSettings.BankCollectionName);
            if (_bank_DB == null)
            {
                return false;
            }

            return true;
        }


        //=====================================
        //Find methods    
        public async Task<List<Client>> FindAllClients()
        {
            List<Client> TeamsList = await _bank_DB.Find(_ => true).ToListAsync();

            return TeamsList;
        }


        public async Task<Client> FindById(int id)
        {
            List<Client> TeamsList = await _bank_DB.Find(_ => true).ToListAsync();

            Client client = TeamsList.Find((a) => a.Id.Equals(id));

            return client;
        }

        public async Task<Client> FindClient(string UserName, string Password)
        {
            List<Client> TeamsList = await _bank_DB.Find(_ => true).ToListAsync();

            Client client = TeamsList.Find((t) => t.UserName == UserName && t.Password == Password);

            return client;
        }

        //=================================================
        //Insert methods
        public async Task<int> AddClient(Client newClient)
        {
            List<Client> TeamsList = await _bank_DB.Find(_ => true).ToListAsync();

            TeamsList.Sort((a, b) => a.Id.CompareTo(b.Id));
            int newClientId;
            if (TeamsList.Count > 0)
            {
                newClientId = TeamsList.Last().Id + 1;
            }
            else
            {
                newClientId = 1;
            }

            

            newClient.Id = newClientId;

            await _bank_DB.InsertOneAsync(newClient);

            return newClientId;
        }

        public async Task<int> AddBankAccount(int clientId, Account account)
        {
            Client client = await FindById(clientId);
            if(!(client is Client)) { return 0; }


            client.Accounts.Sort((a, b) => a.Id.CompareTo(b.Id));
            int newAccountId;
            if (client.Accounts.Count > 0)
            {
                newAccountId = client.Accounts.Last().Id + 1;
            }
            else { newAccountId = 1; }

            account.Id = newAccountId;

            client.Accounts.Add(account);

            await _bank_DB.ReplaceOneAsync(t => t.Id == clientId, client);

            return newAccountId;
        }


        public async Task<int> AddTransaction(int clientId, int accountId, Transaction transaction)
        {
            Client client = await FindById(clientId);            
            if (!(client is Client)) { return 0; }
            Account account = client.Accounts.Find(t => t.Id == accountId);
            if (!(account is Account)) { return 0; }

            account.Transactions.Sort((a, b) => a.Id.CompareTo(b.Id));
            int newTransactionId;
            if (account.Transactions.Count > 0)
            {
                newTransactionId = account.Transactions.Last().Id + 1;
            }
            else { newTransactionId = 1; }

            transaction.Id = newTransactionId;

            account.Transactions.Add(transaction);
            account.Balance += transaction.Amount;
            await _bank_DB.ReplaceOneAsync(t => t.Id == clientId, client);

            return newTransactionId;
        }


        //=============================================
        //Change Methods

        public async Task<bool> ChangeClient(int clientId, Client newClient)
        {
            Client client = await FindById(clientId);
            if (!(client is Client)) { return false; }
            newClient.Id = clientId;
            await _bank_DB.ReplaceOneAsync(t => t.Id == clientId, newClient);
            return true;
        }

        public async Task<bool> ChangeBankAccount(int clientId, int accountId, Account newAccount)
        {
            Client newClient = await FindById(clientId);
            if (!(newClient is Client)) { return false; }

            int accountIndex = newClient.Accounts.FindIndex(t => t.Id == accountId);
            if(accountIndex == -1) { return false; }

            newAccount.Id = accountId;
            newClient.Accounts[accountIndex] = newAccount;

            return await ChangeClient(clientId, newClient); ;

        }


        public async Task<bool> ChangeBankTransfer(int clientId, int accountId,int transferId, Transaction transactiont)
        {
            Client newClient = await FindById(clientId);
            if (!(newClient is Client)) { return false; }

            Account account = newClient.Accounts.Find(t => t.Id == accountId);
            if (!(account is Account)) { return false; }


            int TransferIndex = account.Transactions.FindIndex(t => t.Id == transferId);
            if(TransferIndex == -1) { return false; }

            transactiont.Id = transferId;
            account.Transactions[TransferIndex] = transactiont;

            return await ChangeClient(clientId, newClient);
        }


        //==============================================
        //Delete methods
        public async Task<bool> RemoveClient(int id)
        {
            Client client = await FindById(id);
            if (!(client is Client)) { return false; }

            await _bank_DB.DeleteOneAsync(t => t.Id == id);
            return true;
        }


        public async Task<bool> RemoveClientAccount(int clientId, int accountId)
        {
            Client client = await FindById(clientId);
            if (!(client is Client)) { return false; }

            Account account = client.Accounts.Find(t => t.Id == accountId);
            if (!(account is Account)) { return false; }

            client.Accounts.Remove(account);

            await _bank_DB.ReplaceOneAsync(t => t.Id == clientId, client);

            return true;
        }

        public async Task<bool> RemoveClientTransaction(int clientId, int accountId, int transactionId)
        {
            Client client = await FindById(clientId);
            if (!(client is Client)) { return false; }

            Account account = client.Accounts.Find(t => t.Id == accountId);
            if (!(account is Account)) { return false; }

            Transaction transaction = account.Transactions.Find(t => t.Id == transactionId);
            if (!(transaction is Transaction)) { return false; }

            account.Transactions.Remove(transaction);

            await _bank_DB.ReplaceOneAsync(t => t.Id == clientId, client);

            return true;
        }
    }
}
