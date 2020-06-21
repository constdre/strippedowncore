
import React, { useState, useEffect } from 'react';
import { Redirect } from 'react-router-dom';
import { connect } from 'react-redux';
import ActionStatus from "../ActionStatus";
import { updateShareable } from "../../redux-slices/ShareableSlice";
import { resetUpdateFailed, resetUpdateRedirect } from "../../redux-slices/ProcessStatusSlice";
import { myLog } from "../../../utils";
import { VIEW_SHAREABLES } from '../../util/constants';
import {
    increaseP,
    reduceP,
} from "./manage-methods";



//Function component with hooks and Redux through connect()
const DisplayShareable = (props) => {
    
    const { shareable, 
        updateShareable, 
        redirectUpdate, 
        updateFailed, 
        resetUpdateRedirect, 
        resetUpdateFailed } = props;
    
        window.scrollTo(0, 0);

    //hooks called should be the equivalent in all renders 
    const [isEdit, toggleEdit] = useState(false);//local state
    const isView = !isEdit;
    const paragraphs = shareable.paragraphs;

    useEffect(() => {

        return () => {
            myLog("Leaving update, resetting status-related states")
            resetUpdateRedirect();
            resetUpdateFailed();
        }
    }, []);


    if (redirectUpdate) {
        resetUpdateRedirect();
        return <Redirect to={VIEW_SHAREABLES} />
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

                {updateFailed && <ActionStatus status={updateFailed} />}

                <form name="updateForm" data-action="/Shareable/UpdateShareable" method="post" encType="multipart/form-data">
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
                                    <div id="ParagraphsData[0]">
                                        <input id={"Paragraphs_0__Id"} name={"Paragraphs[0].Id"} hidden />
                                        <textarea readOnly={isView} id="Paragraphs_0__Text" name="Paragraphs[0].Text" rows="8" className={class_text_area}></textarea>
                                    </div>
                                    : //ternary, render many:
                                    paragraphs.map((el, i) => {
                                        return (
                                            <div id={`ParagraphsData[${i}]`}>
                                                <input id={`Paragraphs_${i}__Id`} name={`Paragraphs[${i}].Id`} value={el.id} hidden />
                                                <textarea defaultValue={el.text} readOnly={isView} id={`Paragraphs_${i}__Text`} name={`Paragraphs[${i}].Text`} rows="8" className={class_text_area}></textarea>
                                            </div>
                                        );
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
        const url = postForm.dataset.action

        updateShareable(url, formData);

    }

    return componentMarkup;

}
function mapStateToProps(state) {
    return {
        redirectUpdate:state.processStatus.redirectUpdate,
        updateFailed: state.processStatus.updateFailed
    }
}


export default connect(
    mapStateToProps,
    {
        updateShareable,
        resetUpdateFailed,
        resetUpdateRedirect
    }
)(DisplayShareable);