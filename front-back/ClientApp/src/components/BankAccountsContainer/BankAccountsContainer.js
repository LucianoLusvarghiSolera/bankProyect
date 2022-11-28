import "./BankAccountsContainer.css";
import BankAccountItem from "../BankAccountItem/BankAccountItem.js";
import HeaderBar from "../HeaderBar/HeaderBar.js";
import AccountSideMenu from "../AccountSideMenu/AccountSideMenu.js";

function BankAccountsContainer() {
  return (
    <div className="main-page">
         <AccountSideMenu />
         <main>
           <HeaderBar />
           <section className='bank-accounts-container'>
          <div className='bank-accounts-title'>
            <p>Bank Accounts</p>
            <button className='create-btn'>CREATE</button>
          </div>
          <BankAccountItem />
          <BankAccountItem />
          <BankAccountItem />
        </section>
        </main>
       
    </div>
    
  );
}

export default BankAccountsContainer;
