
import React, { useEffect } from 'react';
import { connect, useDispatch, useSelector } from 'react-redux';
import { Redirect } from 'react-router-dom';
import { addShareable } from "../../redux-slices/ShareableSlice";
import ActionStatus from "../ActionStatus";
import { resetAddRedirect, resetAddFailed } from '../../redux-slices/ProcessStatusSlice';
import { myLog } from "../../../utils";
import { VIEW_SHAREABLES } from '../../util/constants';
import {
    increaseP,
    reduceP,
    browseClick,
    selectImage,
} from "./manage-methods";

//Function component with hooks and Redux hooks 
const CreateShareable = () => {

    window.scrollTo(0, 0);

    const dispatch = useDispatch();
    const redirectAdd = useSelector(state => state.processStatus.redirectAdd);
    const addFailed = useSelector(state => state.processStatus.addFailed);

    useEffect(() => {

        return () => {
            //cleanup function, resets addFailed and addRedirect to null to prevent status showing in the next
            myLog("Leaving create, resetting status-related states");
            dispatch(resetAddFailed());
            dispatch(resetAddRedirect());
        }

    }, []); //executes the body after first render, executes returning function on unmounting

    if (redirectAdd) {
        return <Redirect to={VIEW_SHAREABLES} />
    }

    const componentMarkup = (
        <div className="container-paper">

            <div className="container-center">

                {addFailed && <ActionStatus status={addFailed} />}

                <form name="createForm" data-action="/Shareable/CreateShareable" method="POST" enctype="multipart/form-data">
                    <div className="field-group field-group--medium">
                        <div className="input-wrapper-medium">
                            <input id="Title" name="title" placeholder="Title" className="field-input field-input--title" />
                        </div>
                    </div>

                    <div className="field-group field-group--medium">
                        <label for="Introduction" className="field-label field-label--medium">Introduction</label>
                        <div className="input-wrapper-medium">
                            <textarea id="Introduction" name="Introduction" rows="5" cols="50" className="field-input field-input--area"></textarea>
                        </div>
                    </div>

                    <div className="field-group field-group--medium">
                        <label for="Paragraphs[0].Text" className="field-label field-label--medium">Paragraphs</label>
                        <btn id="btn_add" className="btn btn-icon noselect" onClick={increaseP} style={{ margin: '0 0.5rem' }}>+</btn>
                        <btn id="btn_remove" className="btn btn-icon noselect" onClick={reduceP}>-</btn>
                        <div id="paragraphs_container" className="input-wrapper-medium">
                            <div id="ParagraphsData[0]" className="paragraph-wrapper">
                                <input id={"Paragraphs_0__Id"} name={"Paragraphs[0].Id"} hidden />
                                <textarea id="Paragraphs_0__Text" name="Paragraphs[0].Text" rows="8" cols="50" className="field-input field-input--area"></textarea>
                            </div>
                        </div>
                    </div>

                    <div className="field-group field-group--medium">
                        <input type="file" id="input_image" onChange={selectImage} name="avatar" hidden />
                        <button id="btn_browse" type="button" className="btn" onClick={browseClick}>Select Display Image</button>

                        <div id="div_preview" className="hidden-element vertical-list" style={{ marginTop: '1.5rem' }}>
                            <p id="p_filename" className="field-label field-label--medium"></p>
                            <div className="shareable-large">
                                <div>
                                    <img id="preview_image" className="image" src="~/images/kobe.jpeg" />
                                </div>
                                <div className="s-details">
                                    <p id="preview_title" className="s-title"></p>
                                    <p id="preview_intro" className="s-intro"></p>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div className="form-actions" style={{ marginTop: '5rem' }}>
                        <button type="button" className="btn btn--large1" onClick={handleSubmit}>Submit</button>
                    </div>
                </form>
            </div>
        </div>
    );


    function handleSubmit(e) {

        e.preventDefault();
        const postForm = document.forms.namedItem("createForm");
        myLog(postForm);
        const formData = new FormData(postForm);
        const url = postForm.dataset.action

        //gives the dispatch reference to the async thunk function
        dispatch(addShareable(url, formData));
        
    }

    return componentMarkup;
}

//no need for connect() to connect redux
export default CreateShareable;