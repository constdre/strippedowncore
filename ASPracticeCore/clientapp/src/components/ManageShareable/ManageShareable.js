import React from 'react';
import DisplayShareable from './DisplayShareable';
import CreateShareable from './CreateShareable';

const ManageShareable = (props)=> {

        //location.state holds item data, if it exists then purpose is for displaying that data
        const locationState = props.location.state;
        if (locationState) {
            return <DisplayShareable shareable={locationState.shareable} />
        }
        //otherwise purpose is to add item
        else {
            return <CreateShareable />
        }

}

export default ManageShareable;



