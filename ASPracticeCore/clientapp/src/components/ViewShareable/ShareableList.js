import React, { Component } from 'react';
import { withRouter } from 'react-router-dom';
import ShareableLarge from './ShareableLarge';
import { fetchShareables } from '../../redux-slices/ShareableSlice';
import { connect } from 'react-redux';
import { myLog } from '../../../utils';
import Loader from '../../util/Loader';

const mapStateToProps = state => {
    return {
        isLoading: state.shareable.isLoading,
        shareables: state.shareable.shareables
    };
}

class ShareableList extends Component {

    constructor(props) {
        super(props);
    }
    async componentDidMount() {
        //retrieves user data and stores them in redux state
        this.props.fetchShareables();
    }

    render() {

        const { shareables } = this.props;

        if (shareables.length == 0) {
            return <Loader />
        }
        return (

            <div>
                {
                    shareables.map((el, i) => {


                        let filePath = "";

                        if (el.filePaths.length > 0) {
                            // filePath = "/" + s.filePaths[0].path;
                            filePath = "/" + el.filePaths[0].path;
                        }

                        return <ShareableLarge shareable={el} filePath={filePath} key={i} />
                    }

                    )
                }
                <div className="form-actions form-actions--center">
                    <button id="createShareable" type="button" className="btn btn--large1" onClick={this.showCreateComponent}>Add Shareable</button>
                </div>
            </div>



        );

    }

    showCreateComponent = () => {
        this.props.history.push('/Shareable/CreateShareable');//history, match, location are props automatically attached to children of a router or those rendered through a router
    }

}

//connect/map/attach redux state and action dispatches to react
export default connect(mapStateToProps, { fetchShareables })(withRouter(ShareableList));