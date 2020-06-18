import React from 'react';
import {Switch, Route } from 'react-router-dom';
import ManageShareable from './ManageShareable/ManageShareable';
import ViewShareables from './ViewShareable/ViewShareables';




class UserShareables extends React.Component {

    render() {

        return (

            <Switch>
                <Route path="/Shareable/UserShareables" exact component={ViewShareables} />
                <Route path="/Shareable/UserShareables/:shareableId?" exact component={ManageShareable} />
            </Switch>

        )
    }
    
}

export default UserShareables;



