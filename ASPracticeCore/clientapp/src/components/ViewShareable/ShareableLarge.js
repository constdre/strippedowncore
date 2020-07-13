import React, { Component } from 'react';
import { withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import { myLog } from "../../../utils";
import { deleteShareable } from '../../redux-slices/ShareableSlice';


const mapStateToProps = state => {
    return {
        isManage: state.shareable.isManage
    };
}

class ShareableLarge extends Component {

    constructor(props) {
        super(props);
        this.shareableUrl = this.props.match.url + "/" + this.props.shareable.id;
        this.state = {
            deleteConfirm: false
        }
    }

    render() {

        const { shareable } = this.props;
        let path = this.props.filePath;

        if (!path) {
            path = "/images/kobe.jpeg";
        }

        return (
            <div className="card-shadow">
                <div className="shareable-large">
                    <div>
                        <img className="image" src={path} />
                    </div>
                    <div className="s-details">
                        <div id="sTitle" className="s-title">
                            <span className="s-title__a" onClick={this.openItem}>{shareable.title}</span>
                        </div>
                        <div id="sIntroduction" className="s-intro">{shareable.introduction}</div>

                        {/* Conditional markup: */}
                        {this.deleteFunction()}

                    </div>
                </div>
            </div>
        );
    }

    deleteFunction() {
        if (this.props.isManage && !this.state.deleteConfirm) {
            return (
                <div className="bottommost-actions">
                    <button className="btn btn--adjacent" onClick={this.openItem}>Edit</button>
                    <button className="btn btn--adjacent" onClick={this.deleteConfirmation}>Delete</button>
                </div>
            );
        } else if (this.state.deleteConfirm) {
            myLog("Showing delete confirmation");
            const { title } = this.props.shareable;
            return (
                <div className="bottommost-actions">
                    <p>Are you sure you want to delete {title}?</p>
                    <button className="btn btn--adjacent" onClick={this.deleteCancel}>Back</button>
                    <button className="btn btn--adjacent" onClick={this.deleteItem}>Yes</button>
                </div>
            );
        } else {
            return null;
        }
    }

    openItem = () => {

        //pass data so no need for request to the server 
        const locationWithState = {
            pathname: this.shareableUrl,
            state: {
                shareable: {
                    ...this.props.shareable,
                    filePath: this.props.filePath
                }
            }
        }
        const { history } = this.props;
        history.push(locationWithState);

    };

    deleteConfirmation = () => {
        this.setState({
            deleteConfirm: true
        });
    }
    deleteCancel = () => {
        this.setState({deleteConfirm: false });
    }
    deleteItem = () => {

        const { id, title } = this.props.shareable;
        myLog(`Will now delete item ${title}`);
        const url = "/Shareable/DeleteShareable";
        this.props.deleteShareable(url, { id, title });
    }

}

export default connect(mapStateToProps, { deleteShareable })(withRouter(ShareableLarge));