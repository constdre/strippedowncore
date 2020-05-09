//Script that  attaches the react-built 'Shareable' Components to the Razor View

import React from 'react';
import ReactDOM from 'react-dom';
import ShareableList from './components/ShareableList';


//Id in query params = security in the Authorization API if ever user replaces id value
const paramString = window.location.search;
const urlParams = new URLSearchParams(paramString);
const personName  = urlParams.get("personName");



const url = "/Shareable/GetShareablesOfUser?userId=" + urlParams.get("userId");
displayShareables(url);

async function displayShareables(url) {
    const shareables = await getUserShareables(url);
    ReactDOM.render(<ShareableList shareables={shareables} />, document.getElementById("shareable_items"));
    console.log("react component rendered");
}
async function getUserShareables(url) {
    try {
        const response = await fetch(url);
        const shareables = await response.json();
        console.log("FETCH API TEST", shareables);
        return shareables;
    } catch (err) {
        console.error(err);
    }
}



