
//Redux Toolkit Approach : Concise

import axios from 'axios';
import { createSlice } from '@reduxjs/toolkit';
import * as ProcessStatus from './ProcessStatusSlice';
import { SUCCESS_MESSAGE } from '../util/constants';
import { myLog } from "../../utils";

const initialState = {
    shareables: [],
    isLoading: true,
    redirect: null,
    postStatus: null,
    postSuccess: null,
    isManage: false
}

//comprehensive but succinct:
const shareableSlice = createSlice({
    name: "shareableCRUD",
    initialState,
    reducers: {
        getShareablesSuccess: (state, action) => {
            state.isLoading = false;
            //though it looks mutating, immer library wraps it in a pure function
            state.shareables = action.payload;

        },
        getShareablesFailed: (state, { payload }) => {
            state.errorMessage = payload;
        },

        deleteItem: (state, { payload }) => {
            state.shareables = state.shareables.filter(el => el.id != payload);
        },

        toggleManage: state => {
            state.isManage = !state.isManage;
        }
    }
});

//reducer is the default export
const ShareableReducer = shareableSlice.reducer;
export default ShareableReducer;

//actions from the slice
export const {
    getShareablesSuccess, getShareablesFailed,
    resetPostResponse, resetRedirect,
    toggleManage, deleteItem } = shareableSlice.actions;



//========ASYNC REQUESTS WITH THUNK
export const addShareable = (url, data) => async (dispatch, getState) => {
    myLog("Sending POST request to ", url);
    try {
        const response = await axios.post(url, data);
        const postStatus = response.data;//"success_item1 added!"
        myLog("Status from POST request", postStatus);
        const status = postStatus.split("_")[1];//"item1 added!"

        //success or failed dispatch depending on status from server
        postStatus.toLowerCase().includes(SUCCESS_MESSAGE) ?
            dispatch(ProcessStatus.addSuccess(status)) :
            dispatch(ProcessStatus.addFailed(status));

    } catch (err) {

        const errorMessage = parseError("axios err" + err);
        dispatch(ProcessStatus.addFailed(errorMessage));
    }
}
export const updateShareable = (url, data) => async (dispatch) => {
    myLog("Sending POST request to ", url);
    try {
        const response = await axios.post(url, data);
        const postStatus = response.data;
        myLog("Status from POST request", postStatus);
        const status = postStatus.split("_")[1];

        postStatus.toLowerCase().includes(SUCCESS_MESSAGE) ?
            dispatch(ProcessStatus.updateSuccess(status)) :
            dispatch(ProcessStatus.updateFailed(status));

    } catch (err) {

        const errorMessage = parseError(err);
        dispatch(ProcessStatus.updateFailed(errorMessage));
    }

}
export const deleteShareable = (url, data) => async (dispatch) => {
    myLog("Sending POST request to ", url);
    try {
        const response = await axios.post(url, data);
        const postStatus = response.data;
        myLog("Status from POST request", postStatus);
        const status = postStatus.split("_")[1];


        if (!postStatus.toLowerCase().includes(SUCCESS_MESSAGE)) {
            dispatch(ProcessStatus.deleteFailed(status));
            return;
        }

        dispatch(ProcessStatus.deleteSuccess(status));
        dispatch(deleteItem(data.id));

    } catch (err) {

        const errorMessage = parseError(err);
        dispatch(ProcessStatus.deleteFailed(errorMessage));
    }
}
export const fetchShareables = () => async (dispatch) => {

    try {

        const response = await axios.get("/Shareable/GetShareablesOfUser");
        myLog("Async thunk GetUserShareables", response.data);
        dispatch(getShareablesSuccess(response.data));

    } catch (err) {

        const errorMessage = parseError(err);
        dispatch(getShareablesFailed(errorMessage));
    }

}
//======Helper Methods
function parseError(error) {

    let errorMessage = error;
    myLog("Fetch error", error);

    //perfrom some classification
    if (error.response) {
        //There was a response, error from the server
        errorMessage = "An error in the server occured.";
    } else if (error.request) {
        //No response, error is in the request
        errorMessage = "Bad request.";
    }

    return errorMessage;
}
