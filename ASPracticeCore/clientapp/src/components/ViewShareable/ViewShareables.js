import React from 'react';
import { toggleManage, resetPostResponse } from '../../features/shareable/ShareableSlice'
import ShareableList from './ShareableList';
import { connect } from 'react-redux';
import { handleStatus } from '../../util/manage-methods';
import ActionStatus from "../ActionStatus";
import { myLog } from "../../../utils";
import { bindActionCreators } from 'redux';

class ViewShareables extends React.Component {

    constructor(props) {
        super(props);
        this.showCreateComponent = this.showCreateComponent.bind(this);
        myLog("Here in ViewShareables");

    }

    componentWillUnmount() {
        //resets redirect, postStatus, and postSuccess in the store
        console.log("View Shareable resetting postresponse...");
        resetPostResponse();
    }

    render() {
        const {postStatus} = this.props;
        return (
            <div className="content-container">
                <div id="div_shareables" className="container-narrow">
                    {postStatus && <ActionStatus />}
                    <div className="horizontal-apart">
                        <span className="section-header-2">Your Posts</span>
                        <button className="btn btn--medium" onClick={this.manageShareables}>Manage Items</button>
                    </div>
                    <div id="shareable_items" className="container-shareables-1c">
                        <ShareableList />
                    </div>
                    <div className="form-actions form-actions--center">
                        <button id="createShareable" type="button" className="btn btn--large1" onClick={this.showCreateComponent}>Add Shareable</button>
                    </div>
                </div>
            </div>
        )

    }

    showCreateComponent() {
        //"this" made accessible through .bind(this) in constructor
        this.props.history.push('/Shareable/CreateShareable');//history, match, location are props automatically attached to children of a router or those rendered through a router
    }
    manageShareables = () => {
        // //Arrow function where you don't need to explicitly bind 'this' in the constructor
        this.props.toggleManage();//dispatch to redux state
    }

    async getUserShareables(url) {
        //UNUSED, moved to Redux Thunk async action
        try {
            const response = await fetch(url);
            const shareables = await response.json();
            return shareables;
        } catch (err) {
            console.error(err);
        }
    }
}

const selector = (state) => {
    return {
        redirect: state.shareable.redirect,
        postStatus: state.shareable.postStatus
    }
}
const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({ toggleManage, resetPostResponse }, dispatch)
};
export default connect(selector, mapDispatchToProps)(ViewShareables);