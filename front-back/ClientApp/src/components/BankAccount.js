
import { useState } from "react"

import { GetClientById } from "./client"

const BankAccountBlueprint = {
    id: 0,
    bankName: "",
    balance: 0.0,    
    transactions: []
}


//====================================================================
//Post method

const CreateBankAccount = async ( ClientID, BankName, Balance, SetClientToRefresh ) => {

    const [Account, setAccount] = useState(BankAccountBlueprint)

    setAccount.bankName = BankName;
    setAccount.balance = Balance;
    

    const response = await fetch("api/Client/Add/" + ClientID + "/BankAccount", { method: 'POST', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(Account) });

    const data = await response.json();

    if (response.ok) {        
        setAccount.id = data;
        GetClientById(ClientID, SetClientToRefresh);
    } else {
        console.log("Error: " + data);
    }

}



//===================================================================
//Put methods

const EditBankAccount = async (ClientID, BankAccountID, newBankAccount, SetClientToRefresh) => {

    const [Account, setAccount] = useState(BankAccountBlueprint)

    setAccount(newBankAccount);
    setAccount.id = BankAccountID;

    const response = await fetch("api/Client/Edit/" + ClientID + "/BankAccount/" + BankAccountID, { method: 'PUT', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(Account) });

    if (response.ok) {
        GetClientById(ClientID, SetClientToRefresh);
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}


//=================================================================
//DELETE

const RemoveClient = async (ClientID, BankAccountID, SetClientToRefresh) => {

    const response = await fetch("api/Client/Delete/" + ClientID + "/BankAccount/" + BankAccountID, { method: 'DELETE' });

    if (response.ok) {
        GetClientById(ClientID, SetClientToRefresh);
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}