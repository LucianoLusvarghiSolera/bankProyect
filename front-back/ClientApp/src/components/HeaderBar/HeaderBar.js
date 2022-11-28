import "./HeaderBar.css";
import { setDisplay } from "./../AccountSideMenu/AccountSideMenu.js";
import { useNavigate} from 'react-router-dom';
import * as ImIcons from 'react-icons/im';
import * as AiIcons from 'react-icons/ai';

function getCurrentURL() {
  return window.location.href;
  
}

function HeaderBar() {
  const navigate = useNavigate();
  const navigateNotifications = () =>{
    navigate('/notifications');
  }
  let show = false;
  if(getCurrentURL() === "http://localhost:3000/notifications"){
    show = true;
  }
  else{
    show = false;
  }
  return (
    <div>
      <header>
        <div>
          <ImIcons.ImMenu className="headermenu" onClick={setDisplay}/>       
          {/* <h1 onClick={setDisplay}>Options</h1> */}
          <h2>Real World App</h2>
          <div>
            <button className='new-transaction-btn'>$ NEW</button>
            <AiIcons.AiFillBell size={25} className="notification-adjust" onClick={navigateNotifications}></AiIcons.AiFillBell> {/* onClick we add what needs to redirect to  */}
            {/* <button>Notif</button> */}
          </div>
        </div>
        {show === true &&<div className="header-selection">
          <p>EVERYONE</p>
          <p>FRIENDS</p>
          <p>MINE</p>
        </div>}
        
      </header>
    </div>
  );
}

export default HeaderBar;
