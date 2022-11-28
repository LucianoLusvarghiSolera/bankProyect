using front_back.Database;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace test_backend.Database
{
    [TestClass]
    public class DatabaseUnitTest
    {

        [TestMethod]
        public void ValidateSettingsCreation()
        {

            BankDatabaseSettings db_test = new BankDatabaseSettings();

            Assert.IsNotNull(db_test, "Cant create Settings");

            Assert.IsTrue(db_test.ConnectionString.Equals("mongodb+srv://admin:admin@cluster0.chqxutx.mongodb.net/?retryWrites=true&w=majority"), "ConnectionString changed \n expected: mongodb+srv://admin:admin@cluster0.chqxutx.mongodb.net/?retryWrites=true&w=majority \n obtained: " + db_test.ConnectionString);
            Assert.IsTrue(db_test.DatabaseName.Equals("Team2-Bank-DB"), "DatabaseName changed \n expected: Team2-Bank-DB \n obtained: " + db_test.DatabaseName);
            Assert.IsTrue(db_test.BankCollectionName.Equals("Bank"), "BankCollectionName changed \n expected: Bank \n obtained: " + db_test.BankCollectionName);

        }

        [TestMethod]
        public void ValidateConnection()
        {

            DB_controller db_test = DB_controller.instance();
            bool result = db_test.DB_connect();

            Assert.IsTrue(result, "Fail to connect MongoDB");

        }




        [TestMethod]
        public async Task ValidateAddandRemoveClient()
        {

            DB_controller db_test = DB_controller.instance();
            db_test.DB_connect();

            //Create a client
            Client client = new Client();
            client.FirstName = "Luciano";
            client.LastName = "Lusvarghi";
            client.UserName = "Chunfly";
            client.Password = "A123456*";

            //Check client initial state
            Assert.IsTrue(client.FirstName.Equals("Luciano"), "Client FirstName expected: Luciano -- obtained: " + client.FirstName);
            Assert.IsTrue(client.LastName.Equals("Lusvarghi"), "Client LastName expected: Lusvarghi -- obtained: " + client.LastName);
            Assert.IsTrue(client.UserName.Equals("Chunfly"), "Client UserName expected: Chunfly -- obtained: " + client.UserName);
            Assert.IsTrue(client.Password.Equals("A123456*"), "Client Password expected: A123456* -- obtained: " + client.Password);

            Assert.IsTrue(client.Accounts.Count.Equals(0), "Client Accounts count expected: 1 -- obtained: " + client.Accounts.Count.ToString());


            //Add client and recover it
            int clientId = await db_test.AddClient(client);
            Client checkClient = await db_test.FindById(clientId);

            //Check client recover state
            Assert.IsTrue(checkClient.FirstName.Equals("Luciano"), "Client FirstName expected: Luciano -- obtained: " + checkClient.FirstName);
            Assert.IsTrue(checkClient.LastName.Equals("Lusvarghi"), "Client LastName expected: Lusvarghi -- obtained: " + checkClient.LastName);
            Assert.IsTrue(checkClient.UserName.Equals("Chunfly"), "Client UserName expected: Chunfly -- obtained: " + checkClient.UserName);
            Assert.IsTrue(checkClient.Password.Equals("A123456*"), "Client Password expected: A123456* -- obtained: " + checkClient.Password);

            //Create a account
            Account account = new Account();
            account.BankName = "Santander";
            account.Balance = 1500.0;

            //Put account
            int AccountId = await db_test.AddBankAccount(clientId, account);

            //Recover client with new account
            checkClient = await db_test.FindById(clientId);

            //Check account state
            Assert.IsTrue(checkClient.Accounts.Count.Equals(1), "Client Accounts count expected: 1 -- obtained: " + checkClient.Accounts.Count.ToString());
            Assert.IsTrue(checkClient.Accounts[0].BankName.Equals("Santander"), "Client Account Bank expected: Santander -- obtained: " + checkClient.Accounts[0].BankName);
            Assert.IsTrue(checkClient.Accounts[0].Balance.Equals(1500.0), "Client Account Balance expected: 1500.0 -- obtained: " + checkClient.Accounts[0].Balance.ToString());
            Assert.IsTrue(checkClient.Accounts[0].Transactions.Count.Equals(0), "Account transaction count expected: 0 -- obtained: " + checkClient.Accounts[0].Transactions.Count.ToString());

            //Create Transaction
            Transaction transaction = new Transaction();
            transaction.RecipientsName = "Rangu";
            transaction.Concept = "Funny guy";
            transaction.Amount = 500.0;

            int TransactionId = await db_test.AddTransaction(clientId, AccountId, transaction);
            //Recover client with new transaction
            checkClient = await db_test.FindById(clientId);
            Assert.IsTrue(checkClient.Accounts[0].Transactions.Count.Equals(1), "Account transaction count expected: 1 -- obtained: " + checkClient.Accounts[0].Transactions.Count.ToString());

            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].RecipientsName.Equals("Rangu"), "Transaction RecipientsName expected: Rangu -- obtained: " + checkClient.Accounts[0].Transactions[0].RecipientsName);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].Concept.Equals("Funny guy"), "Transaction Concept expected: Funny guy -- obtained: " + checkClient.Accounts[0].Transactions[0].Concept);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].Amount.Equals(500.0), "Transaction Amount expected: 500.0 -- obtained: " + checkClient.Accounts[0].Transactions[0].Amount.ToString());


            Assert.IsTrue(checkClient.Accounts[0].Balance.Equals(1500.0 + 500.0), "Transaction Amount expected: 2000.0 -- obtained: " + checkClient.Accounts[0].Balance.ToString());


            //Remove Transaction
            await db_test.RemoveClientTransaction(clientId, AccountId, TransactionId);
            //Recover client with out transaction
            checkClient = await db_test.FindById(clientId);
            Assert.IsTrue(checkClient.Accounts[0].Transactions.Count.Equals(0), "Client Accounts count expected: 0 -- obtained: " + checkClient.Accounts[0].Transactions.Count.ToString());


            //Remove account
            await db_test.RemoveClientAccount(clientId, AccountId);
            //Recover client with out account
            checkClient = await db_test.FindById(clientId);
            Assert.IsTrue(checkClient.Accounts.Count.Equals(0), "Client Accounts count expected: 0 -- obtained: " + checkClient.Accounts.Count.ToString());

            //Remove client
            await db_test.RemoveClient(clientId);
            //Check the client is removed
            checkClient = await db_test.FindById(clientId);
            Assert.IsNull(checkClient);


        }


        [TestMethod]
        public async Task ValidateFindClient()
        {

            DB_controller db_test = DB_controller.instance();
            db_test.DB_connect();

            Client client = new Client();
            client.FirstName = "Luciano";
            client.LastName = "Lusvarghi";
            client.UserName = "Chunfly";
            client.Password = "A123456*";
            client.Accounts.Clear();

            Assert.IsTrue(client.FirstName.Equals("Luciano"), "Client FirstName expected: Luciano -- obtained: " + client.FirstName);
            Assert.IsTrue(client.LastName.Equals("Lusvarghi"), "Client LastName expected: Lusvarghi -- obtained: " + client.LastName);
            Assert.IsTrue(client.UserName.Equals("Chunfly"), "Client UserName expected: Chunfly -- obtained: " + client.UserName);
            Assert.IsTrue(client.Password.Equals("A123456*"), "Client Password expected: A123456* -- obtained: " + client.Password);

            int clientId = await db_test.AddClient(client);

            Client checkClient = await db_test.FindClient("Chunfly", "A123456*");

            Assert.IsTrue(checkClient.FirstName.Equals("Luciano"), "Client FirstName expected: Luciano -- obtained: " + checkClient.FirstName);
            Assert.IsTrue(checkClient.LastName.Equals("Lusvarghi"), "Client LastName expected: Lusvarghi -- obtained: " + checkClient.LastName);
            Assert.IsTrue(checkClient.UserName.Equals("Chunfly"), "Client UserName expected: Chunfly -- obtained: " + checkClient.UserName);
            Assert.IsTrue(checkClient.Password.Equals("A123456*"), "Client Password expected: A123456* -- obtained: " + checkClient.Password);


            checkClient = await db_test.FindClient("Chunfly", "A654321*");
            Assert.IsFalse(checkClient is Client, "Returned client from invalid Find operation return a client");


            await db_test.RemoveClient(clientId);

            checkClient = await db_test.FindById(clientId);
            Assert.IsNull(checkClient);


        }



        [TestMethod]
        public async Task ValidateChangeClient()
        {

            DB_controller db_test = DB_controller.instance();
            db_test.DB_connect();

            //Create a client
            Client client = new Client();
            client.FirstName = "Luciano";
            client.LastName = "Lusvarghi";
            client.UserName = "Chunfly";
            client.Password = "A123456*";
            int clientId = await db_test.AddClient(client);

            //Create a account
            Account account = new Account();
            account.BankName = "Santander";
            account.Balance = 1500.0;
            int AccountId = await db_test.AddBankAccount(clientId, account);

            //Create Transaction
            Transaction transaction = new Transaction();
            transaction.RecipientsName = "Rangu";
            transaction.Concept = "Funny guy";
            transaction.Amount = 500.0;
            int TransactionId = await db_test.AddTransaction(clientId, AccountId, transaction);

            //Recover the client with all modifications
            Client checkClient = await db_test.FindById(clientId);

            //Change client settings
            checkClient.FirstName = "Emilio";
            checkClient.LastName = "Pedro";
            //Put the changed client
            await db_test.ChangeClient(clientId, checkClient);

            //check the posted new client
            checkClient = await db_test.FindById(clientId);
            Assert.IsTrue(checkClient.FirstName.Equals("Emilio"), "Client FirstName expected: Emilio -- obtained: " + checkClient.FirstName);
            Assert.IsTrue(checkClient.LastName.Equals("Pedro"), "Client LastName expected: Pedro -- obtained: " + checkClient.LastName);
            Assert.IsTrue(checkClient.Accounts.Count.Equals(1), "Client Accounts count expected: 1 -- obtained: " + checkClient.Accounts.Count.ToString());
            Assert.IsTrue(checkClient.Accounts[0].BankName.Equals("Santander"), "Client Account Bank expected: Santander -- obtained: " + checkClient.Accounts[0].BankName);
            Assert.IsTrue(checkClient.Accounts[0].Balance.Equals(1500.0 + 500.0), "Client Account Balance expected: 2000.0 -- obtained: " + checkClient.Accounts[0].Balance.ToString());
            Assert.IsTrue(checkClient.Accounts[0].Transactions.Count.Equals(1), "Account transaction count expected: 1 -- obtained: " + checkClient.Accounts[0].Transactions.Count.ToString());
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].RecipientsName.Equals("Rangu"), "Transaction RecipientsName expected: Rangu -- obtained: " + checkClient.Accounts[0].Transactions[0].RecipientsName);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].Concept.Equals("Funny guy"), "Transaction Concept expected: Funny guy -- obtained: " + checkClient.Accounts[0].Transactions[0].Concept);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].Amount.Equals(500.0), "Transaction Amount expected: 500.0 -- obtained: " + checkClient.Accounts[0].Transactions[0].Amount.ToString());

            //Change client account settings
            checkClient.Accounts[0].BankName = "Saltenitas";
            checkClient.Accounts[0].Balance = 5500.0;
            //Put the changed client account
            await db_test.ChangeBankAccount(clientId, AccountId, checkClient.Accounts[0]);
            //check the posted new client
            checkClient = await db_test.FindById(clientId);           
            Assert.IsTrue(checkClient.Accounts.Count.Equals(1), "Client Accounts count expected: 1 -- obtained: " + checkClient.Accounts.Count.ToString());
            Assert.IsTrue(checkClient.Accounts[0].BankName.Equals("Saltenitas"), "Client Account Bank expected: Saltenitas -- obtained: " + checkClient.Accounts[0].BankName);
            Assert.IsTrue(checkClient.Accounts[0].Balance.Equals(5500.0), "Client Account Balance expected: 5500.0 -- obtained: " + checkClient.Accounts[0].Balance.ToString());
            Assert.IsTrue(checkClient.Accounts[0].Transactions.Count.Equals(1), "Account transaction count expected: 1 -- obtained: " + checkClient.Accounts[0].Transactions.Count.ToString());
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].RecipientsName.Equals("Rangu"), "Transaction RecipientsName expected: Rangu -- obtained: " + checkClient.Accounts[0].Transactions[0].RecipientsName);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].Concept.Equals("Funny guy"), "Transaction Concept expected: Funny guy -- obtained: " + checkClient.Accounts[0].Transactions[0].Concept);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].Amount.Equals(500.0), "Transaction Amount expected: 500.0 -- obtained: " + checkClient.Accounts[0].Transactions[0].Amount.ToString());

            //Change client transfer settings
            checkClient.Accounts[0].Transactions[0].Amount = -700;
            checkClient.Accounts[0].Transactions[0].Concept = "refound game";
            checkClient.Accounts[0].Transactions[0].RecipientsName = "Steam";
            //Put the changed client transfer
            await db_test.ChangeBankTransfer(clientId, AccountId, TransactionId, checkClient.Accounts[0].Transactions[0]);
            checkClient = await db_test.FindById(clientId);            
            Assert.IsTrue(checkClient.Accounts[0].Transactions.Count.Equals(1), "Account transaction count expected: 1 -- obtained: " + checkClient.Accounts[0].Transactions.Count.ToString());
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].RecipientsName.Equals("Steam"), "Transaction RecipientsName expected: Steam -- obtained: " + checkClient.Accounts[0].Transactions[0].RecipientsName);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].Concept.Equals("refound game"), "Transaction Concept expected: refound game -- obtained: " + checkClient.Accounts[0].Transactions[0].Concept);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].Amount.Equals(-700), "Transaction Amount expected: -700 -- obtained: " + checkClient.Accounts[0].Transactions[0].Amount.ToString());



            //Remove client
            await db_test.RemoveClient(clientId);
            //Check the client is removed
            checkClient = await db_test.FindById(clientId);
            Assert.IsNull(checkClient);

        }


        
        [TestMethod]
        public async Task CreateUser()
        {

            DB_controller db_test = DB_controller.instance();
            db_test.DB_connect();

            //Create a client
            Client client = new Client();
            client.FirstName = "Emilio";
            client.LastName = "Vini";
            client.UserName = "Alpha";
            client.Password = "8526AAA*";

            //Check client initial state
            Assert.IsTrue(client.FirstName.Equals("Emilio"), "Client FirstName expected: Luciano -- obtained: " + client.FirstName);
            Assert.IsTrue(client.LastName.Equals("Vini"), "Client LastName expected: Lusvarghi -- obtained: " + client.LastName);
            Assert.IsTrue(client.UserName.Equals("Alpha"), "Client UserName expected: Chunfly -- obtained: " + client.UserName);
            Assert.IsTrue(client.Password.Equals("8526AAA*"), "Client Password expected: A123456* -- obtained: " + client.Password);

            Assert.IsTrue(client.Accounts.Count.Equals(0), "Client Accounts count expected: 1 -- obtained: " + client.Accounts.Count.ToString());


            //Add client and recover it
            int clientId = await db_test.AddClient(client);
            Client checkClient = await db_test.FindById(clientId);

            //Check client recover state
            Assert.IsTrue(checkClient.FirstName.Equals("Emilio"), "Client FirstName expected: Luciano -- obtained: " + checkClient.FirstName);
            Assert.IsTrue(checkClient.LastName.Equals("Vini"), "Client LastName expected: Lusvarghi -- obtained: " + checkClient.LastName);
            Assert.IsTrue(checkClient.UserName.Equals("Alpha"), "Client UserName expected: Chunfly -- obtained: " + checkClient.UserName);
            Assert.IsTrue(checkClient.Password.Equals("8526AAA*"), "Client Password expected: A123456* -- obtained: " + checkClient.Password);

            //Create a account
            Account account = new Account();
            account.BankName = "Santander";
            account.Balance = 1500.0;

            //Put account
            int AccountId = await db_test.AddBankAccount(clientId, account);

            account = new Account();
            account.BankName = "IberCaja";
            account.Balance = 15500.0;

            //Put account
            int AccountId2 = await db_test.AddBankAccount(clientId, account);

            //Recover client with new account
            checkClient = await db_test.FindById(clientId);

            //Check account state
            Assert.IsTrue(checkClient.Accounts.Count.Equals(2), "Client Accounts count expected: 2 -- obtained: " + checkClient.Accounts.Count.ToString());

            Assert.IsTrue(checkClient.Accounts[0].BankName.Equals("Santander"), "Client Account Bank expected: Santander -- obtained: " + checkClient.Accounts[0].BankName);
            Assert.IsTrue(checkClient.Accounts[0].Balance.Equals(1500.0), "Client Account Balance expected: 1500.0 -- obtained: " + checkClient.Accounts[0].Balance.ToString());
            Assert.IsTrue(checkClient.Accounts[0].Transactions.Count.Equals(0), "Account transaction count expected: 0 -- obtained: " + checkClient.Accounts[0].Transactions.Count.ToString());

            Assert.IsTrue(checkClient.Accounts[1].BankName.Equals("IberCaja"), "Client Account Bank expected: Santander -- obtained: " + checkClient.Accounts[0].BankName);
            Assert.IsTrue(checkClient.Accounts[1].Balance.Equals(15500.0), "Client Account Balance expected: 1500.0 -- obtained: " + checkClient.Accounts[0].Balance.ToString());
            Assert.IsTrue(checkClient.Accounts[1].Transactions.Count.Equals(0), "Account transaction count expected: 0 -- obtained: " + checkClient.Accounts[0].Transactions.Count.ToString());


            //Create Transaction
            Transaction transaction = new Transaction();
            transaction.RecipientsName = "Rangu";
            transaction.Concept = "Funny guy";
            transaction.Amount = 500.0;

            int TransactionId = await db_test.AddTransaction(clientId, AccountId, transaction);

            transaction = new Transaction();
            transaction.RecipientsName = "Eric";
            transaction.Concept = "Table Game";
            transaction.Amount = -570.0;

            int TransactionId2 = await db_test.AddTransaction(clientId, AccountId, transaction);

            transaction = new Transaction();
            transaction.RecipientsName = "Doc";
            transaction.Concept = "Doc";
            transaction.Amount = -50.0;

            int TransactionId3 = await db_test.AddTransaction(clientId, AccountId2, transaction);

            //Recover client with new transaction
            checkClient = await db_test.FindById(clientId);
            Assert.IsTrue(checkClient.Accounts[0].Transactions.Count.Equals(2), "Account transaction count expected: 1 -- obtained: " + checkClient.Accounts[0].Transactions.Count.ToString());
            Assert.IsTrue(checkClient.Accounts[1].Transactions.Count.Equals(1), "Account transaction count expected: 1 -- obtained: " + checkClient.Accounts[0].Transactions.Count.ToString());


            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].RecipientsName.Equals("Rangu"), "Transaction RecipientsName expected: Rangu -- obtained: " + checkClient.Accounts[0].Transactions[0].RecipientsName);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].Concept.Equals("Funny guy"), "Transaction Concept expected: Funny guy -- obtained: " + checkClient.Accounts[0].Transactions[0].Concept);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[0].Amount.Equals(500.0), "Transaction Amount expected: 500.0 -- obtained: " + checkClient.Accounts[0].Transactions[0].Amount.ToString());

            Assert.IsTrue(checkClient.Accounts[0].Transactions[1].RecipientsName.Equals("Eric"), "Transaction RecipientsName expected: Rangu -- obtained: " + checkClient.Accounts[0].Transactions[0].RecipientsName);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[1].Concept.Equals("Table Game"), "Transaction Concept expected: Funny guy -- obtained: " + checkClient.Accounts[0].Transactions[0].Concept);
            Assert.IsTrue(checkClient.Accounts[0].Transactions[1].Amount.Equals(-570.0), "Transaction Amount expected: 500.0 -- obtained: " + checkClient.Accounts[0].Transactions[0].Amount.ToString());

            Assert.IsTrue(checkClient.Accounts[1].Transactions[0].RecipientsName.Equals("Doc"), "Transaction RecipientsName expected: Rangu -- obtained: " + checkClient.Accounts[0].Transactions[0].RecipientsName);
            Assert.IsTrue(checkClient.Accounts[1].Transactions[0].Concept.Equals("Doc"), "Transaction Concept expected: Funny guy -- obtained: " + checkClient.Accounts[0].Transactions[0].Concept);
            Assert.IsTrue(checkClient.Accounts[1].Transactions[0].Amount.Equals(-50.0), "Transaction Amount expected: 500.0 -- obtained: " + checkClient.Accounts[0].Transactions[0].Amount.ToString());



            Assert.IsTrue(checkClient.Accounts[0].Balance.Equals(1500.0 + 500.0 - 570.0), "Transaction Amount expected: 2000.0 -- obtained: " + checkClient.Accounts[0].Balance.ToString());
            Assert.IsTrue(checkClient.Accounts[1].Balance.Equals(15500.0 - 50.0), "Transaction Amount expected: 2000.0 -- obtained: " + checkClient.Accounts[0].Balance.ToString());



        }



    }
}
