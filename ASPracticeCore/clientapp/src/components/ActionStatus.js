import React from 'react';
import { useSelector } from 'react-redux'
import { myLog } from '../../utils';

const ActionStatus = ({status}) => {

    const postSuccess = useSelector(state=>state.processStatus.postSuccess);
    myLog("ActionStatus status & postSuccess: ",status,postSuccess)

    //No status to show
    if(status === null){
        return null;
    }

    //conditional styling
    const backgroundColor = postSuccess ?
        "green" : "red";
    
    //inline style
    const style = {
        padding: "2rem",
        marginBottom:"2rem",
        backgroundColor,
        color:"white",
        borderRadius: "15px",
        fontFamily: ' "Roboto",sans-serif '
    }

    return (
        <div style={style}>
            <h2>{status}</h2>
        </div>
    );

}

export default ActionStatus;