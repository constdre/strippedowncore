import React, { Component } from 'react';
import '../../../wwwroot/css/shareable.css';

class ShareableLarge extends Component {

    render() {
        console.log("shareable " + this.props.key, this.props.shareable);
        let path = this.props.filePath;
        if (!path) {
            path = "/images/kobe.jpeg";
        }
        console.log("image path: ", path);

        let shareableUrl = "/Shareable/ManageShareable?shareableId=" + this.props.shareable.id;
        return (
            <div className="children-mbs-nl">
                <div className="shareable-large">
                    <div className="shareable-group">
                        <img id="thumbnail" className="s-image-large" src={path} />
                    </div>
                    <div className="shareable-group s-details">
                        <p id="sTitle" className="s-title"><a className="s-title__a" href={shareableUrl}>{this.props.shareable.title}</a></p>
                        <p id="sIntroduction" className="s-intro">{this.props.shareable.introduction}</p>
                    </div>
                </div>
            </div>

        );
    }
}

export default ShareableLarge;