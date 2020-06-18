import React, { Component } from 'react';
import ShareableLarge from './ShareableLarge';
import {fetchShareables} from '../../features/shareable/ShareableSlice';
import { connect } from 'react-redux';
import { myLog } from '../../../utils';


const mapStateToProps = state=>{
    return{
        isLoading: state.shareable.isLoading,
        shareables:state.shareable.shareables
    };
}

class ShareableList extends Component {

    constructor(props){
        super(props);
    }
    async componentDidMount(){
        //retrieves user data and stores them in redux state
        this.props.fetchShareables();
        // console.log("ShareableList -> the passed shareables:", this.props.shareables);
    }

    render() {

        const {shareables} = this.props;

        if(shareables.length==0){
            return <h1>Loading...</h1>
        }

        return (

            shareables.map((el, i) => {

               
                let filePath = "";

                if (el.filePaths.length>0) {
                    // filePath = "/" + s.filePaths[0].path;
                    filePath = "/"+el.filePaths[0].path;
                }

                return <ShareableLarge shareable={el} filePath={filePath} key={i} />
            })


        );

    }
    
    componentDidUpdate(){
        myLog("ShareableList just updated!");
    }

}

//connect/map/attach redux state and action dispatches to react
export default connect(mapStateToProps,{fetchShareables})(ShareableList);