
using front_back.Database;

using Microsoft.AspNetCore.Mvc;

namespace front_back.Controllers
{

    public class ValidateUser
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }


    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : Controller
    {

        public ClientController() { }

        //==========================================================
        //GetMethods

        // Returning a list of Clients
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {            
            DB_controller dB_Controller = DB_controller.instance();
            
            List<Client> ClientList = await dB_Controller.FindAllClients();

            return StatusCode(StatusCodes.Status200OK, ClientList);
        }


        // Returning a client with username and Password
        [HttpGet]
        [Route("Validate")]
        public async Task<IActionResult> ValidateClient([FromBody] ValidateUser User)
        {
            DB_controller dB_Controller = DB_controller.instance();

            Client client = await dB_Controller.FindClient(User.UserName, User.Password);

            bool result = client is Client;

            return result ? StatusCode(StatusCodes.Status200OK, client) : StatusCode(StatusCodes.Status400BadRequest, "Cant find user match");
        }


        // Returning a client with ID
        [HttpGet]
        [Route("All/{ID:int}")]
        public async Task<IActionResult> ClientId(int ID)
        {
            DB_controller dB_Controller = DB_controller.instance();

            Client client = await dB_Controller.FindById(ID);
            bool result = client is Client;

            return result ? StatusCode(StatusCodes.Status200OK, client) : StatusCode(StatusCodes.Status400BadRequest, "Cant find user match");
        }

        //=====================================================
        //Post Methods


        // Creating a Client
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddClient([FromBody] Client newClient)
        {
            DB_controller dB_Controller = DB_controller.instance();

            int newClientId = await dB_Controller.AddClient(newClient);
            bool result = newClientId != 0;
            return result ? StatusCode(StatusCodes.Status200OK, newClientId) : StatusCode(StatusCodes.Status400BadRequest, "Cant create Client");
        }

        // Creating a Bank Account
        [HttpPost]
        [Route("Add/{ID:int}/BankAccount")]
        public async Task<IActionResult> AddBankAccount(int ID, [FromBody] Account newAccount)
        {
            DB_controller dB_Controller = DB_controller.instance();

            int BankAccountID = await dB_Controller.AddBankAccount(ID, newAccount);
            bool result = BankAccountID != 0;
            return result? StatusCode(StatusCodes.Status200OK, BankAccountID) : StatusCode(StatusCodes.Status400BadRequest, "Cant create BankAccount");
        }
        
        // Creating a Transaction
        [HttpPost]
        [Route("Add/{ID:int}/BankAccount/{AccountID:int}/Transaction")]
        public async Task<IActionResult> AddTransaction(int ID, int AccountID, [FromBody] Transaction newTransaction)
        {
            DB_controller dB_Controller = DB_controller.instance();

            int TransactionID = await dB_Controller.AddTransaction(ID, AccountID, newTransaction);
            bool result = TransactionID != 0;
            return result ? StatusCode(StatusCodes.Status200OK, TransactionID) : StatusCode(StatusCodes.Status400BadRequest, "Cant create Transaction");
        }
        

        //=======================================================
        //Put methods

        // Change a Client
        [HttpPut]
        [Route("Edit/{ID:int}")]
        public async Task<IActionResult> ChangeClient(int ID, [FromBody] Client client)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.ChangeClient(ID, client);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Client ID");

        }

        
        // Change a Bank Account
        [HttpPut]
        [Route("Edit/{ID:int}/BankAccount/{AccountID:int}")]
        public async Task<IActionResult> ChangeBankAccount(int ID, int AccountID, [FromBody] Account account)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.ChangeBankAccount(ID, AccountID, account);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Client ID or Invalid Bank Account ID");

        }

        
        // Change a Bank Account
        [HttpPut]
        [Route("Edit/{ID:int}/BankAccount/{AccountID:int}/Transaction/{TransactionID:int}")]
        public async Task<IActionResult> ChangeBankTransfer(int ID, int AccountID,int TransactionID, [FromBody] Transaction transaction)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.ChangeBankTransfer(ID, AccountID, TransactionID, transaction);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Client ID or Invalid Bank Account ID or Invalid Transaction ID");

        }
        
        //=========================================================
        //Delete methods


        // Deleting a Client
        [HttpDelete]
        [Route("Delete/{ID:int}")]
        public async Task<IActionResult> RemoveClient(int ID)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.RemoveClient(ID);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Client ID");
        }

        
        // Deleting a Client
        [HttpDelete]
        [Route("Delete/{ID:int}/BankAccount/{AccountID:int}")]
        public async Task<IActionResult> RemoveBankAccount(int ID, int AccountID)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.RemoveClientAccount(ID, AccountID);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Client ID or Invalid Bank Account ID");
        }

        
        // Deleting a Client
        [HttpDelete]
        [Route("Delete/{ID:int}/BankAccount/{AccountID:int}/Transaction/{TransactionID:int}")]
        public async Task<IActionResult> RemoveBankTransfer(int ID, int AccountID, int TransactionID)
        {
            DB_controller dB_Controller = DB_controller.instance();

            bool result = await dB_Controller.RemoveClientTransaction(ID, AccountID, TransactionID);

            return result ? StatusCode(StatusCodes.Status200OK, "ok") : StatusCode(StatusCodes.Status400BadRequest, "Invalid Client ID or Invalid Bank Account ID or Invalid Transaction ID");
        }
        
    }
}
