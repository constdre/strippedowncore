import React from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import UserShareables from './UserShareables';
import ManageShareable from './ManageShareable/ManageShareable';


//Your React App - Entrypoint 

const ShareableModule = () => {
    //Server points to here and React settles the further front-end navigation depending on the address path
    return (
        <BrowserRouter>
            <div>
                <Switch>
                    <Route path="/Shareable/UserShareables/:shareableId?" component={UserShareables} />
                    <Route exact path="/Shareable/CreateShareable" exact component={ManageShareable} />
                </Switch>
            </div>
        </BrowserRouter>
    );
}

export default ShareableModule;