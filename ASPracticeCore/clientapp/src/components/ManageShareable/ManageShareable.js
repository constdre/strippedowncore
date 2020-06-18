import React, { useState, useEffect } from 'react';
import DisplayShareable from './DisplayShareable';
import CreateShareable from './CreateShareable';

class ManageShareable extends React.Component {

    render() {

        //location.state holds item data, if it exists then purpose is for displaying that data
        const locationState = this.props.location.state;
        if (locationState) {
            return <DisplayShareable shareable={locationState.shareable} />
        }
        //otherwise purpose is to add item
        else {
            return <CreateShareable />
        }

    }

}

export default ManageShareable;



