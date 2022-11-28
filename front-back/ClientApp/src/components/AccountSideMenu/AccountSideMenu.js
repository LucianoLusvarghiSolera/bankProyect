import "./AccountSideMenu.css";
import {Routes, Route, useNavigate} from 'react-router-dom';
import * as ImIcons from 'react-icons/im';
import * as AiIcons from 'react-icons/ai';
import * as BiIcons from 'react-icons/bi';

export function setDisplay() {
    if (document.getElementById("aside").style.display === "flex") {
        document.getElementById("aside").style.display = "none";
    } else {
        document.getElementById("aside").style.display = "flex";
    }
}

function AccountSideMenu() {
    const accountFirstName = 'John';
    const accountLastName = 'Doe';
    const accountEmail = 'johndoe@generic.com';
    const balance = 10000;
    const navigate = useNavigate();
    const navigateHome = () =>{
      navigate('/home');
    }
    const navigateMyAccount = () =>{
      navigate('/myaccount');
    }
    const navigateBankAccount = () =>{
      navigate('/bankaccount');
    }
    const navigateNotifications = () =>{
      navigate('/notifications');
    }
    const navigateLogOut = () =>{
      navigate('/');
    }
  return (
    <aside id="aside">
      <section className="account-credentials">
        <div className='ph-img'>IMG</div>
        <div>
          <p>{accountFirstName} {accountLastName}</p>
          <p className='account-email'>{accountEmail}</p>
        </div>
      </section>
      <div className="account-balance">
        <h1>${balance}</h1>
        <p>Account Balance</p>
      </div>
      <section className="account-options">
        <button onClick={navigateHome}>
          <div className="icons"><ImIcons.ImHome3 size={30}/></div>
          <p>Home</p>
        </button>
        <button onClick ={navigateMyAccount}>
        <div className="icons"><ImIcons.ImUser size={30}/></div>
          <p>My Account</p>
        </button>
        <button onClick = {navigateBankAccount}>
        <div className="icons"><ImIcons.ImLibrary size={30}/></div>
          <p>Bank Accounts</p>
        </button>
        <button onClick={navigateNotifications}>
        <div className="icons"><AiIcons.AiFillBell size={30}/></div>
          <p>Notifications</p>
        </button>
        <button onClick ={navigateLogOut}>
        <div className="icons"><BiIcons.BiLogOut size={30}/></div>
          <p>Logout</p>
        </button>
      </section>
    </aside>
  );
}

export default AccountSideMenu;
