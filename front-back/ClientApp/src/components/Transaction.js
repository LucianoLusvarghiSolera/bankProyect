
import { useState } from "react"

import { GetClientById } from "./client"

const TransactionBlueprint = {
    id: 0,
    recipientsName: "",
    concept: "",
    amount: 0.0    
}


//====================================================================
//Post method

const CreateTransaction = async (ClientID, BankAccountID, RecipientsName, Concept, Amount, SetClientToRefresh) => {

    const [Transaction, setTransaction] = useState(TransactionBlueprint)

    setTransaction.recipientsName = RecipientsName;
    setTransaction.concept = Concept;
    setTransaction.amount = Amount;

    const response = await fetch("api/Client/Add/" + ClientID + "/BankAccount/" + BankAccountID + "/Transaction", { method: 'POST', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(Transaction) });

    const data = await response.json();

    if (response.ok) {
        setTransaction.id = data;
        GetClientById(ClientID, SetClientToRefresh);
    } else {
        console.log("Error: " + data);
    }

}



//===================================================================
//Put methods

const EditTransaction = async (ClientID, BankAccountID, TranasctionID, newTransaction, SetClientToRefresh) => {

    const [Transaction, setTransaction] = useState(TransactionBlueprint)

    setTransaction(newTransaction);
    setTransaction.id = TranasctionID;

    const response = await fetch("api/Client/Edit/" + ClientID + "/BankAccount/" + BankAccountID + "/Transaction/" + TranasctionID, { method: 'PUT', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(Transaction) });

    if (response.ok) {
        GetClientById(ClientID, SetClientToRefresh);
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}


//=================================================================
//DELETE

const RemoveTransaction = async (ClientID, BankAccountID, TranasctionID, SetClientToRefresh) => {

    const response = await fetch("api/Client/Delete/" + ClientID + "/BankAccount/" + BankAccountID + "/Transaction/" + TranasctionID, { method: 'DELETE' });

    if (response.ok) {
        GetClientById(ClientID, SetClientToRefresh);
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}