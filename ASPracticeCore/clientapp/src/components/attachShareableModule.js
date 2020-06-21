import React from 'react';
import ReactDOM from 'react-dom';
import ShareableModule from './ShareableModule';
import { Provider } from 'react-redux';
import {configureStore, getDefaultMiddleware} from '@reduxjs/toolkit';
import ShareableReducer from '../redux-slices/ShareableSlice';
import ProcessStatusReducer from '../redux-slices/ProcessStatusSlice';
import {myLog} from "../../utils";

const middleware = [
    ...getDefaultMiddleware()
    //custom middlewares here
];

const Store = configureStore({
    reducer:{
        //combined into one root reducer
        shareable:ShareableReducer,
        processStatus:ProcessStatusReducer
    },
    middleware
});

ReactDOM.render(
    <Provider store={Store}>
        <ShareableModule />
    </Provider>,
    document.querySelector('#react_root'));