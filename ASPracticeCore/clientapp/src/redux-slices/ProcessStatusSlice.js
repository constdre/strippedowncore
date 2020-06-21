//Regular Redux no-toolkit approach


const initialState = {
    //CRUD success message unified to "postSuccess" because they're observed by just 1 view.
    postSuccess: null,
    postStatus: null,
    //request failed messages have own states for their own listening views
    addFailed: null,
    updateFailed: null,
    deleteFailed: null,

    //"redirect" state separate between add and delete for better scaling
    redirectAdd: null,
    redirectUpdate: null
}

export default function ProcessStatusReducer(state = initialState, action) {

    //=======Traditional Reducer

    switch (action.type) {

        case ADD_SUCCESS: return Object.assign({}, state, {
            postSuccess: true,
            postStatus: action.payload,
            redirectAdd: true
        })
        case ADD_FAILED: return {
            ...state,
            postSuccess: false,
            addFailed: action.payload
        }
        case UPDATE_SUCCESS: return {
            ...state,
            postSuccess: true,
            postStatus: action.payload,
            redirectUpdate: true
        }
        case UPDATE_FAILED: return {
            ...state,
            postSuccess: false,
            updateFailed: action.payload
        }
        case DELETE_SUCCESS: return {
            ...state,
            postSuccess: true,
            postStatus: action.payload
        }
        case DELETE_FAILED: return {
            ...state,
            postSuccess: false,
            postStatus: action.payload
        }

        //Resets called after showing a status message

        case RESET_POST_SUCCESS: return {
            ...state,
            postSuccess: null,
            postStatus: null
        }
        case RESET_ADD_FAILED: return {
            ...state,
            addFailed: null
        }
        case RESET_UPDATE_FAILED: return {
            ...state,
            updateFailed: null
        }

        //Redirect Resets called after redirecting

        case RESET_ADD_REDIRECT: return {
            ...state,
            redirectAdd: null
        }
        case RESET_UPDATE_REDIRECT: return {
            ...state,
            redirectUpdate: null
        }

    }
    return state;
}


//verbose, everything is separate - 1 constant : 1 function creator

const ADD_SUCCESS = "add_success";
const ADD_FAILED = "add_failed";
const UPDATE_SUCCESS = "update_success";
const UPDATE_FAILED = "update-failed";
const DELETE_SUCCESS = "delete_success";
const DELETE_FAILED = "delete_failed";

const RESET_POST_SUCCESS = "reset_post_success"
const RESET_ADD_FAILED = "reset_add_failed";
const RESET_UPDATE_FAILED = "reset_update_failed";
const RESET_ADD_REDIRECT = "reset_add_redirect";
const RESET_UPDATE_REDIRECT = "reset_update_redirect";

export function addSuccess(payload) {
    return {
        type: ADD_SUCCESS,
        payload
    }
}
export function addFailed(payload) {
    return {
        type: ADD_FAILED,
        payload
    }
}
export function updateSuccess(payload) {
    return {
        type: UPDATE_SUCCESS,
        payload
    }
}
export function updateFailed(payload) {
    return {
        type: UPDATE_FAILED,
        payload
    }
}
export function deleteSuccess(payload) {
    return {
        type: DELETE_SUCCESS,
        payload
    }
}
export function deleteFailed(payload) {
    return {
        type: DELETE_FAILED,
        payload
    }
}

//====Resets after showing a status message

export function resetPostSuccess() {
    return {
        type: RESET_POST_SUCCESS
    }
}
export function resetAddFailed() {
    return {
        type: RESET_ADD_FAILED
    }
}
export function resetUpdateFailed() {
    return {
        type: RESET_UPDATE_FAILED
    }
}

export function resetAddRedirect() {
    return {
        type: RESET_ADD_REDIRECT
    }
}
export function resetUpdateRedirect() {
    return {
        type: RESET_UPDATE_REDIRECT
    }
}
