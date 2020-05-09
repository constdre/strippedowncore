import React, { Component } from 'react';
import ShareableLarge from './ShareableLarge';
import '../../../wwwroot/css/general.css';

class ShareableList extends Component {

    //constructor() {
    //    super(props);
    //    console.log("constructor check", this.props.shareables);
    //}

    render() {
        console.log("passed shareables:", this.props.shareables);

        return (

            this.props.shareables.map((s, i) => {

                console.log("shareable" + i, s);
                let filePath = "";

                if (s.filePaths.length>0) {
                    filePath = "/" + s.filePaths[0].path;
                }

                return <ShareableLarge shareable={s} filePath={filePath} key={i} />
            })


        );

        //return (
        //    this.props.shareables.map((s, i) => {
        //        <ShareableLarge shareable={s} key={i} className="children-mbs-nl" />
        //    })
        //);
    }

}

export default ShareableList;