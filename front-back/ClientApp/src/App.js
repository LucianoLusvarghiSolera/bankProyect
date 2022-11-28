import "./App.css";
import MainPage from "./components/MainPage/MainPage.js";
import SignUpPage from "./components/SignUpPage/SignUpPage.js";
import BankAccountsContainer from "./components/BankAccountsContainer/BankAccountsContainer";
import UserSettingsMenu from "./components/UserSettingsMenu/UserSettingsMenu";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Notifications from "./components/Notifications/Notifications";

function App() {
    return (
        <>
            <div className="App">
                <Router>
                    <Routes>
                        <Route path="/" exact element={<SignUpPage />} />
                        <Route path="/home" exact element={<MainPage />} />
                        <Route path="/myaccount" exact element={<UserSettingsMenu />} />
                        <Route path="/bankaccount" exact element={<BankAccountsContainer />} />
                        <Route path="/notifications" exact element={<Notifications />} />
                    </Routes>
                </Router>
            </div>
        </>
    );
}

export default App;
