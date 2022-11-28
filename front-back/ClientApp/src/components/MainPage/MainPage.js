import "./MainPage.css";
import HeaderBar from "../HeaderBar/HeaderBar.js";
import AccountSideMenu from "../AccountSideMenu/AccountSideMenu.js";


function MainPage() {
  return (
    <div className="main-page">
      <AccountSideMenu />
      <main>
        <HeaderBar />
        
      </main>
    </div>
  );
}

export default MainPage;
