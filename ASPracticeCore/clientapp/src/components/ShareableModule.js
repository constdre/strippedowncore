import React from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import UserShareables from './UserShareables';
import ManageShareable from './ManageShareable/ManageShareable';
import '../../../wwwroot/css/shareable.css'



class ShareableModule extends React.Component {
    //Your React App
    render() {
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
}

export default ShareableModule;