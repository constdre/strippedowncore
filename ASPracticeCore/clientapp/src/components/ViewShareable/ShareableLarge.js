import React, { Component } from 'react';
import {withRouter } from 'react-router-dom';
import {connect} from 'react-redux';
import {myLog} from "../../../utils";
import { deleteShareable } from '../../redux-slices/ShareableSlice';


const mapStateToProps = state =>{
    return{
        isManage:state.shareable.isManage
    };
}

class ShareableLarge extends Component {
    
    constructor(props){
        super(props);
        this.shareableUrl = this.props.match.url+"/"+this.props.shareable.id;
    }
    
    render() {

        let {shareable} = this.props;
        let path = this.props.filePath;

        if (!path) {
            path = "/images/kobe.jpeg";
        }

        return (
            <div className="component-card">
                <div className="shareable-large">
                    <div>
                        <img className="image" src={path} />
                    </div>
                    <div className="s-details">
                        <p id="sTitle" className="s-title">
                            <span className="s-title__a" onClick={this.openItem}>{shareable.title}</span>
                        </p>
                        <p id="sIntroduction" className="s-intro">{shareable.introduction}</p>
                        <div id="filler"></div>
                        
                        {/* conditional rendering */}
                        {this.props.isManage &&
                            <div className="bottommost-actions">
                                <button className="btn btn--adjacent" onClick={this.openItem}>Edit</button>
                                <button className="btn btn--adjacent" onClick={this.deleteItem}>Delete</button>
                            </div>
                        }
                    </div>
                </div>
            </div>
        );
    }

    openItem = ()=>{

        myLog("Clcked url:",this.shareableUrl);

        //pass data so no need for request to the server 
        const locationWithState = {
            pathname:this.shareableUrl,
            state:{
                shareable : {
                    ...this.props.shareable,
                    filePath : this.props.filePath
                }
            }
        }
        const {history} = this.props;
        history.push(locationWithState);

    };

    deleteItem = ()=>{

        
        const {id,title} = this.props.shareable;
        myLog(`Will now delete item ${title}`);
        const url = "/Shareable/DeleteShareable";
        this.props.deleteShareable(url,{id, title});
    }

}

export default connect(mapStateToProps,{deleteShareable})(withRouter(ShareableLarge));