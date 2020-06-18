import React from 'react';
import ReactDOM from 'react-dom';
import ShareableModule from './ShareableModule';
import { Provider } from 'react-redux';
import {configureStore, getDefaultMiddleware} from '@reduxjs/toolkit';
import shareableReducer from '../features/shareable/ShareableSlice';
import {myLog} from "../../utils";

//style loader injects css in <style> tag to html, use mini-css-extract-plugin for production
// import '../../../wwwroot/css/general.css'
const middleware = [
    ...getDefaultMiddleware()
    //custom middlewares here
];
const Store = configureStore({
    reducer:{
        shareable:shareableReducer
        //can add more, automatically combined into one root reducer
    },
    middleware
});

ReactDOM.render(
    <Provider store={Store}>
        <ShareableModule />
    </Provider>,
    document.querySelector('#react_root'));