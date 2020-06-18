
import React,{useEffect} from 'react';
import { connect, useDispatch } from 'react-redux';
import { Redirect } from 'react-router-dom';
import { createShareable, resetRedirect, resetPostResponse } from "../../features/shareable/ShareableSlice";
import ActionStatus from "../ActionStatus";
import {
    increaseP,
    reduceP,
    browseClick,
    selectImage,
    handleStatus
} from "../../util/manage-methods";
import {myLog} from "../../../utils";

//Function-type Component
const CreateShareable = ({redirect, createShareable, resetRedirect}) => {

    const dispatch = useDispatch();

    if (redirect) {

        resetRedirect();
        return <Redirect to="/Shareable/UserShareables" />
    }

    // useEffect(() => {
    //     //cleanup function, resets redirect:false and postStatus:null
    //     return () => {
    //         console.log("Leaving, resetting postResponse")
    //         dispatch(resetPostResponse());
    //     }
    // });

    const componentMarkup = (
        <div className="container-paper">

            {/* <ActionStatus /> */}

            <div className="container-center">
                <form name="createForm" data-action="/Shareable/CreateShareableAsync" method="POST" enctype="multipart/form-data">
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
                            <textarea id="Paragraphs_0__Text" name="Paragraphs[0].Text" rows="8" cols="50" className="field-input field-input--area"></textarea>
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

        //build form data
        // const form = document.getElementById("form_create");
        // const formElements = form.elements;
        // const id = formElements.namedItem("Id").value;
        // const title = formElements.namedItem("Title").value;
        // const introduction = formElements.namedItem("Introduction").value;
        // const paragraphs = [];
        // const paragraphNodes = document.querySelectorAll("textarea[name^=Paragraphs");
        // for(let i=0; i<paragraphNodes.length; i++){
        //     paragraphs.push(paragraphNodes[i].value);
        // }
        e.preventDefault();
        const postForm = document.forms.namedItem("createForm");
        myLog(postForm);
        const formData = new FormData(postForm);
        myLog("formData before POST", formData);
        const url = postForm.dataset.action
        
        createShareable(url,formData);

    }

    return componentMarkup;


}
function mapStateToProps(state) {
    return {
        redirect: state.shareable.redirect
    }
}


export default connect(mapStateToProps,{createShareable, resetRedirect})(CreateShareable);