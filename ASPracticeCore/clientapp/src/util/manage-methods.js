import axios from 'axios';
import { SUCCESS_MESSAGE, FAILED_MESSAGE } from './constants';
import React from 'react';


//=========Exported Methods
export function increaseP() {

    const parent = document.getElementById("paragraphs_container");
    //child paragraphs:
    const paragraphs = document.querySelectorAll("#paragraphs_container > [id^=Paragraphs]");

    //attr values:
    const length = paragraphs.length;
    const id = `Paragraphs_${length}__Text`;//CollectionName_Index__Property
    const name = `Paragraphs[${length}].Text`;//CollectionName[Index]__Property


    //Copy default paragraph and update attr 
    const paragraph = paragraphs[0].cloneNode();
    paragraph.value = null;
    paragraph.setAttribute("id", id);
    paragraph.setAttribute("name", name);
    paragraph.setAttribute("style", "margin-top:1rem");
    paragraph.setAttribute("defaultValue","");
    //attach to parent
    parent.appendChild(paragraph);

}

export function reduceP() {

    const parent = document.getElementById("paragraphs_container");
    //get updated number each call
    const paragraphs = document.querySelectorAll("#paragraphs_container > [id^=Paragraphs]");
    const length = paragraphs.length;

    //return on 1 default paragraph left
    if (length == 1) {
        return;
    }

    //remove last one:
    const lastEl = paragraphs[paragraphs.length - 1];
    parent.removeChild(lastEl);


}


export function browseClick() {
    document.getElementById("input_image").click();
}
export function selectImage() {

    const container = document.getElementById("div_preview");
    const inputImage = document.getElementById("input_image")
    const fileName = document.getElementById("p_filename");



    //replace any showing preview
    if (document.getElementById("shareable_preview")) {
        const childNodes = container.childNodes;
        container.removeChild(childNodes[childNodes.length - 1]);
    }

    let fileReader = new FileReader();
    fileReader.onload = evt => populatePreview(evt);
    fileReader.readAsDataURL(inputImage.files[0]);

    fileName.textContent = "Preview shareable: " + inputImage.files.item(0).name;

    //reveal the preview
    div_preview.classList.remove("hidden-element");

}

export function handleStatus(status) {

    //builds the "success/failed" status UI component

    if (!status) {
        return;
    }

    //settle the alert color
    const color = status.includes(SUCCESS_MESSAGE) ?
        "green" : "red";
    const style = {
        padding: "2rem",
        backgroundColor: color,
        borderRadius: "15px",
        fontFamily: ' "Roboto",sans-serif '
    }


    //get the message part after the '_'
    const message = status.split("_")[1];//orig: "success_item1 was added"
    return (
        <div style={style}>
            <h2>{message}</h2>
        </div>
    );

}



//======Local Helper Functions

function populatePreview(evt) {

    const txtTitle = document.getElementById("Title");
    const txtIntro = document.getElementById("Introduction");
    const previewTitle = document.getElementById("preview_title");
    const previewImage = document.getElementById("preview_image");
    const previewIntro = document.getElementById("preview_intro");


    previewTitle.textContent = txtTitle.value;
    previewIntro.textContent = txtIntro.value;
    previewImage.src = evt.target.result;

}