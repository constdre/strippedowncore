import React from 'react';
import ShareableList from './ShareableList';
import Loader from '../../util/Loader';
import ActionStatus from "../ActionStatus";
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { toggleManage, resetPostResponse } from '../../redux-slices/ShareableSlice'
import { resetPostSuccess } from '../../redux-slices/ProcessStatusSlice';
import { myLog } from "../../../utils";

//Class component style
class ViewShareables extends React.Component {

    constructor(props) {
        super(props);
        this.showCreateComponent = this.showCreateComponent.bind(this);
    }
    componentDidMount() {
        window.scrollTo(0, 0);
    }
    componentDidUpdate() {
        //react hooks eliminates this kind of redundancies
        window.scrollTo(0, 0);
    }
    componentWillUnmount() {
        //reset postStatus and postSuccess to stop showing alert the next time.
        resetPostSuccess();
    }

    render() {

        const { postStatus } = this.props;
        
        return (
            <div className="content-container">
                <div id="div_shareables" className="container-narrow">
                    {postStatus && <ActionStatus status={postStatus}/>}
                    <div className="horizontal-apart">
                        <span className="section-header-2">Your Posts</span>
                        <button className="btn btn--medium" onClick={this.manageShareables}>Manage Items</button>
                    </div>
                    
                    <div id="shareable_items" className="container-shareables-1c">
                        <ShareableList />
                    </div>
                </div>
            </div>
        )

    }

    showCreateComponent() {
        //"this" is the component given by .bind()
        this.props.history.push('/Shareable/CreateShareable');//history, match, location are props automatically attached to children of a router or those rendered through a router
    }
    manageShareables = () => {
        //Arrow function where you don't need to explicitly bind 'this' in the constructor
        this.props.toggleManage();
    }

}

const mapStateToProps = (state) => {
    return {
        postStatus: state.processStatus.postStatus,
        itemsLength: state.shareable.shareables.length
    }
}
const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({ toggleManage, resetPostResponse }, dispatch)
};
export default connect(mapStateToProps, mapDispatchToProps)(ViewShareables);