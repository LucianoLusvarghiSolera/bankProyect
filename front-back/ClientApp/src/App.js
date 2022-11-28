import React, { useState, useEffect } from 'react';
import { Table } from "reactstrap"

const App = () => {

    const [Client, setClient] = useState([])

    const getTeams = async () => {

        const response = await fetch("api/client/All");

        console.log("Test2");
        console.log(response);
        if (response.ok) {

            const data = await response.json();
            console.log(data);
            setClient(data);

        } else {
            console.log("Error al acceder a la base de datos");
        }

    }

    useEffect(() => {

        getTeams()

    }, [])

    return (
        <div>
            <text>
                Hello World - This is the Fist page of this App
            </text>
            
            <Table striped responsive>
                <thead >
                    <tr>
                        <th>Id</th>
                        <th>FirstName</th>
                        <th>LastName</th>
                        <th>UserName</th>
                        <th>Password</th>
                    </tr>
                </thead>
                {
                    Client.map((Client) => (
                        <tbody>
                            <tr>
                                <td>{Client.id}</td>
                                <td>{Client.firstName}</td>
                                <td>{Client.lastName}</td>
                                <td>{Client.userName}</td>
                                <td>{Client.password}</td>
                            </tr>
                            <tr>
                                <td colSpan="5">
                                    BankAccounts
                                </td>
                            </tr>
                            <tr>
                                <td colSpan="5">
                                    <Table striped responsive>
                                        <thead >
                                            <tr>
                                                <th>Id</th>
                                                <th>BankName</th>
                                                <th>Balance</th>
                                            </tr>
                                        </thead>
                                        {
                                            Client.accounts.map((Accounts) => (
                                                <tbody>
                                                    <tr>
                                                        <td>{Accounts.id}</td>
                                                        <td>{Accounts.bankName}</td>
                                                        <td>{Accounts.balance}</td>
                                                    </tr>
                                                    <tr>
                                                        <td colSpan="3">
                                                            Transactions
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colSpan="3">
                                                            <Table striped responsive>
                                                                <thead >
                                                                    <tr>
                                                                        <th>Id</th>
                                                                        <th>RecipientsName</th>
                                                                        <th>Concept</th>
                                                                        <th>Amount</th>
                                                                    </tr>
                                                                </thead>
                                                                {
                                                                    Accounts.transactions.map((Transaction) => (
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>{Transaction.id}</td>
                                                                                <td>{Transaction.recipientsName}</td>
                                                                                <td>{Transaction.concept}</td>
                                                                                <td>{Transaction.amount}</td>
                                                                            </tr>

                                                                        </tbody>
                                                                    ))
                                                                }
                                                            </Table>
                                                        </td>
                                                    </tr>

                                                </tbody>
                                            ))
                                        }
                                    </Table>
                                </td>
                            </tr>

                        </tbody>
                    ))
                }
            </Table>

        </div>

    );
  
}


export default App;