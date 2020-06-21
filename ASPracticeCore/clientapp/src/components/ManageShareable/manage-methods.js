//Methods shared by Shareable forms, both add and update

//=========Exported Methods
export function increaseP() {

    const parent = document.getElementById("paragraphs_container");

    //child paragraphs:
    const paragraphs = document.querySelectorAll("#paragraphs_container > [id^=ParagraphsData]")
    const length = paragraphs.length;

    //Duplicate
    const index = 0; //index of markup to be duplicated
    const newParagraph = paragraphs[index].cloneNode(true);
    newParagraph.setAttribute("id", `ParagraphsData[${length}]`);
    newParagraph.setAttribute("style", "margin-top:1rem");//harmless top margin
    console.log("CLONED NODE ", newParagraph);

    //new attributes for the cloned elements
    //for el.Id prop
    const idKey = `Paragraphs_${length}__Id`;//CollectionName_Index__PropertyName
    const nameKey = `Paragraphs[${length}].Id`;//CollectionName[Index]__PropertyName
    //for el.Text prop
    const idText = `Paragraphs_${length}__Text`;
    const nameText = `Paragraphs[${length}].Text`;

    //update
    const paragraphId = newParagraph.querySelector(`#Paragraphs_${index}__Id`); 
    console.log("CLONED paragraphId",paragraphId);
    paragraphId.value = null;
    paragraphId.setAttribute("id", idKey);
    paragraphId.setAttribute("name", nameKey);

    const paragraphText = newParagraph.querySelector(`#Paragraphs_${index}__Text`);
    console.log("CLONED text",paragraphText);
    paragraphText.setAttribute("id", idText);
    paragraphText.setAttribute("name", nameText);
    paragraphText.value = null;

    //attach to parent
    parent.appendChild(newParagraph);
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