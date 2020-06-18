import React, { useEffect } from 'react';
import { useSelector } from 'react-redux'
import { resetPostResponse } from '../features/shareable/ShareableSlice';
import { myLog } from '../../utils';

const ActionStatus = () => {

    const postStatus = useSelector(state => state.shareable.postStatus);
    const postSuccess = useSelector(state=>state.shareable.postSuccess);
    console.log("ActionStatus postStatus & postSuccess: ",postStatus,postSuccess)



    //No status to show
    if(postStatus === null){
        return null;
    }



    //conditional styling
    const color = postSuccess ?
        "green" : "red";
    const style = {
        padding: "2rem",
        marginBottom:"2rem",
        backgroundColor: color,
        color:"white",
        borderRadius: "15px",
        fontFamily: ' "Roboto",sans-serif '
    }

    return (
        <div style={style}>
            <h2>{postStatus}</h2>
        </div>
    );

}
export default ActionStatus;