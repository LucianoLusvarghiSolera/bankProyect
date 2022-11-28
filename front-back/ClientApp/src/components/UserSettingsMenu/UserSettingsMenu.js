import "./UserSettingsMenu.css";
import React from 'react';
import {useState} from "react";
import HeaderBar from "../HeaderBar/HeaderBar.js";
import AccountSideMenu from "../AccountSideMenu/AccountSideMenu.js";

function UserSettingsMenu() {

  const[inputs,setInputs] = useState({});
  const[load,setLoad] = useState(true);
  let firstName,lastName;
  console.log(load);

  const handleChange = (event) => {
    event.preventDefault();
    const name = event.target.name;
    const value = event.target.value;
    //console.log(setInputs.username.target.name);
    setInputs(values => ({...values,[name]: value}))
      
    
    if(setInputs !== ('') ){  
      setLoad(false);
    }
    else{
      
      //event.target.value = null;
      setLoad(true);
    }
    
  }

  const handleClick= (event) =>{
   
      if(inputs.firstname === ('') || inputs.lastname === ('')){
        alert("First name or Last name cannot be empty");
      }    
      else{
        firstName = inputs.firstname;
        lastName = inputs.lastname
        setInputs('');
        event.preventDefault();
        console.log(firstName);
        console.log(lastName);
        setLoad(true);
      }

      
      
   
  }


  return (
    <div className="main-page">
        <AccountSideMenu />
        <main>
          <HeaderBar />
          <div className="form-body">
                <p>User Settings</p>
              <form>
                <div className="form-main-body">
                  <div>IMG</div>
                  <div className="form-inputs">
                    <input type="text" placeholder="First name" required name="firstname" value={inputs.firstname || ""} onChange={handleChange}></input>
                    <input type="text" placeholder="Last name" required name="lastname" value={inputs.lastname || ""} onChange={handleChange}></input>
                    <input type="email" placeholder="Email address" required name="emailaddress" value={inputs.emailaddress || ""} onChange={handleChange}></input>
                    <input type="tel" placeholder="Phone number" required name="phonenumber"value={inputs.phonenumber || ""} onChange={handleChange}></input>
                  </div>
                </div>
                <button type="submit" onClick={handleClick} disabled ={load}>Save</button>
              </form>
           </div>
        </main>
      </div>
    
  );
}

export default UserSettingsMenu;
