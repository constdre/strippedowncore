
import React, { useState } from 'react';

import { Redirect } from 'react-router-dom';
import { connect, useDispatch } from 'react-redux';
import { updateShareable, resetPostResponse, resetRedirect } from "../../features/shareable/ShareableSlice";
import ActionStatus from "../ActionStatus";
import { myLog } from "../../../utils";

import {
    increaseP,
    reduceP,
    handleStatus
} from "../../util/manage-methods";



//Function component - promoted approach moving forward
const DisplayShareable = ({ shareable, redirect, updateShareable, resetRedirect }) => {

    //hooks called should be the same in every render
    const [isEdit, toggleEdit] = useState(false);//local state
    const isView = !isEdit;
    const paragraphs = shareable.paragraphs;
    // const dispatch = useDispatch();

    // useEffect(() => {
    //     //cleanup function, resets redirect:false and postStatus:null
    //     return () => {
    //         console.log("Leaving, resetting postResponse")
    //         dispatch(resetPostResponse());
    //     }
    // });

    if (redirect) {
        resetRedirect();
        return <Redirect to="/Shareable/UserShareables" />
    }

    //classes for view setting:
    let class_text_area = "noedit noedit--area";
    let class_title = "noedit noedit--title";

    if (isEdit) {
        //classes for edit setting:
        class_text_area = "field-input field-input--area ";
        class_title = "field-input field-input--title";
    }

    const componentMarkup = (
        <div className="container-paper">
            <div className="container-center">

                {/* <ActionStatus /> */}

                <form name="updateForm" data-action="/Shareable/UpdateShareableAsync" method="post" encType="multipart/form-data">
                    <input name="Id" id="Id" value={shareable.id} hidden />
                    <div className="field-group field-group--medium">
                        <div className="input-wrapper-medium horizontal-apart">
                            <div id="filler" data-purpose="so title is middle and button in end"></div>
                            <input id="Title" readOnly={isView} defaultValue={shareable.title} name="Title" placeholder="Title" className={class_title} />
                            <button class="btn" type="button" onClick={() => { toggleEdit(!isEdit) }} style={{}}>Edit</button>
                        </div>
                    </div>
                    <div className="field-group field-group--medium">
                        {isEdit && <label for="Introduction" className="field-label field-label--medium">Introduction</label>}
                        <div className="input-wrapper-medium">
                            <textarea name="Introduction" id="Introduction" readOnly={isView} defaultValue={shareable.introduction} rows="5" cols="50" className={class_text_area}></textarea>
                        </div>
                    </div>

                    <div className="field-group field-group--medium">
                        {isEdit && //show add/reduce paragraph controls
                            <div>
                                <label for="Paragraphs[0].Text" className="field-label field-label--medium">Paragraphs</label>
                                <btn id="btn_add" className="btn btn-icon noselect" onClick={increaseP} style={{ margin: '0 0.5rem' }}>+</btn>
                                <btn id="btn_remove" className="btn btn-icon noselect" onClick={reduceP}>-</btn>
                            </div>
                        }
                        <div id="paragraphs_container" className="input-wrapper-medium">
                            {
                                paragraphs.length <= 0 ?
                                    <textarea readOnly={isView} id="Paragraphs_0__Text" name="Paragraphs[0].Text" rows="8" className={class_text_area}></textarea>
                                    : //ternary, render many
                                    paragraphs.map((el, i) => {
                                        return <textarea defaultValue={el.text} readOnly={isView} id={`Paragraphs_${i}__Text`} name={`Paragraphs[${i}].Text`} rows="8" className={class_text_area}></textarea>
                                    })
                            }
                        </div>
                    </div>

                    {isEdit && //the form actions
                        <div className="form-actions" style={{ marginTop: '5rem' }}>
                            <button type="button" className="btn btn--large1" onClick={handleSubmit}>Submit</button>
                        </div>
                    }

                </form>

            </div>
        </div>
    );
    function handleSubmit(e) {

        e.preventDefault();
        const postForm = document.forms.namedItem("updateForm");
        myLog(postForm);

        const formData = new FormData(postForm);
        myLog("formData before POST", formData);
        const url = postForm.dataset.action

        updateShareable(url, formData);

    }

    function iterateParagraphs(paragraphs) {
        return paragraphs.map((el, i) => {
            return <textarea defaultValue={el.text} readOnly={isView} id={`Paragraphs_${i}__Text`} name={`Paragraphs[${i}].Text`} rows="8" className={class_text_area}></textarea>
        })
    }


    return componentMarkup;

}
function mapStateToProps(state) {
    return {
        redirect: state.shareable.redirect
    }
}


export default connect(mapStateToProps, { updateShareable, resetRedirect })(DisplayShareable);