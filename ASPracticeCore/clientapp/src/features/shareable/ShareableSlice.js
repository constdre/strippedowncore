
import axios from 'axios';
import { createSlice, createAction, findNonSerializableValue } from '@reduxjs/toolkit';
import { SUCCESS_MESSAGE } from '../../util/constants';
import { myLog } from "../../../utils";

const initialState = {
    shareables: [],
    isLoading: true,
    redirect: null,
    postStatus: null,
    postSuccess: null,
    showStatus:null,
    isManage: false
}
const shareableSlice = createSlice({
    name: "shareableCRUD",
    initialState,
    reducers: {
        getShareablesSuccess: (state, action) => {
            state.isLoading = false;
            //though it looks mutating, still pure through immer lib under
            state.shareables = action.payload;

        },
        getShareablesFailed: (state, { payload }) => {
            state.errorMessage = payload;
        },
        addSuccess: (state, { payload }) => {
            state.redirect = true;
            state.postStatus = payload;
            state.postSuccess = true
        },
        addFailed: (state, { payload }) => {
            state.redirect = false;
            state.postStatus = payload;
            state.postSuccess = false;
        },
        updateSuccess: (state, { payload }) => {
            state.redirect = true;
            state.postStatus = payload;
            state.postSuccess = true
        },
        updateFailed: (state, { payload }) => {
            state.redirect = false;
            state.postStatus = payload;
            state.postSuccess = false;
        },
        deleteSuccess: (state, { payload }) => {
            state.postStatus = payload;
            state.postSuccess = true;

        },

        deleteFailed: (state, { payload }) => {
            state.postStatus = payload;
            state.postSuccess = false;
        },

        resetPostResponse: (state) => {
            state.postStatus = null;
            state.postSuccess = null;
            state.redirect = false;
        },
        resetRedirect:(state)=>{
            state.redirect = false;
        },
        toggleManage: state => {
            state.isManage = !state.isManage;
        }
    }
});

//reducer is the default export
const shareableReducer = shareableSlice.reducer;
export default shareableReducer;

//actions from the slice
export const {
    getShareablesSuccess, getShareablesFailed,
    addSuccess, addFailed,
    updateSuccess, updateFailed,
    deleteSuccess, deleteFailed,
    resetPostResponse, resetRedirect,
    toggleManage } = shareableSlice.actions;


//========ASYNC REQUESTS WITH THUNK
export const createShareable = (url, data) => async (dispatch) => {
    try {
        myLog("Sending POST request to ", url);
        const response = await axios.post(url, data);
        const postStatus = response.data;//"success_item1 added!"
        myLog("Status from POST request", postStatus);
        const message = postStatus.split("_")[1];//"item1 added!"

        //success or failed dispatch depending on status from server
        postStatus.includes(SUCCESS_MESSAGE) ?
            dispatch(addSuccess(message)) :
            dispatch(addFailed(message));

    } catch (err) {

        const errorMessage = parseError(err);
        dispatch(addFailed(errorMessage));
    }
}
export const updateShareable = (url, data) => async (dispatch) => {
    try {
        const response = await axios.post(url, data);
        const postStatus = response.data;
        myLog("Status from POST request", postStatus);
        const message = postStatus.split("_")[1];

        postStatus.includes(SUCCESS_MESSAGE) ?
            dispatch(updateSuccess(message)) :
            dispatch(updateFailed(message));

    } catch (err) {

        const errorMessage = parseError(err);
        dispatch(updateFailed(errorMessage));
    }

}
export const fetchShareables = () => async (dispatch) => {

    try {

        const response = await axios.get("/Shareable/GetShareablesOfUser");
        myLog("Fetchin data in thunk async method", response.data);
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
