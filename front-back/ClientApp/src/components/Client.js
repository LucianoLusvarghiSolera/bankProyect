
import { useState } from "react"

const ClientBlueprint = {
    id: 0,
    firstName: "",
    lastName: "",
    userName: "",
    password: "",
    accounts: []
}

const ValidateClientBluePrint = {

    UserName: "",
    Password: ""

}


//=====================================================================
//Get Methods

const GetClientById = async (ID, SetResultClient) => {

    const response = await fetch("api/Client/All/" + ID);

    if (response.ok) {
        const data = await response.json();
        SetResultClient(data);
    } else {
        console.log("Error: Cant find the give user");
    }

}


const GetValidatedClient = async (UserName, Password, SetResultClient) => {

    const [client, setClient] = useState(ValidateClientBluePrint)

    setClient.userName = UserName;
    setClient.password = Password;

    const response = await fetch("api/Client/Validate", { method: 'GET', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(client) });

    if (response.ok) {
        const data = await response.json();
        SetResultClient(data);
    } else {
        console.log("Error: Cant find the give user");
    }

}


//====================================================================
//Post method

const CreateClient = async (FirstName, LastName, UserName, Password, SetResultClient) => {

    const [client, setClient] = useState(ClientBlueprint)

    setClient.firstName = FirstName;
    setClient.lastName = LastName;
    setClient.userName = UserName;
    setClient.password = Password;

    const response = await fetch("api/Client/Add", { method: 'POST', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(client) });

    if (response.ok) {
        const data = await response.json();
        setClient.id = data;
        SetResultClient(client);
    } else {
        console.log("Error: Cant find the give user");
    }

}


//===================================================================
//Put methods

const EditClient = async ( ClientID, newClient, SetResultClient ) => {

    const [client, setClient] = useState(ClientBlueprint)

    setClient(newClient);
    setClient.id = ClientID;

    const response = await fetch("api/Client/Edit/" + ClientID, { method: 'PUT', headers: { 'Content-Type': 'application/json;charset=utf-8' }, body: JSON.stringify(client) });

    if (response.ok) {
        SetResultClient(client);
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}

//=================================================================
//DELETE

const RemoveClient = async ( ClientID ) => {

    const response = await fetch("api/Client/Delete/" + ClientID, { method: 'DELETE' });

    if (response.ok) {
       
    } else {
        const data = await response.json();
        console.log("Error: " + data);
    }

}