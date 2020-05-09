const ManageShareable = () => {
    return (
        <div className="container-paper">
            <div className="container-center">
                <p className="header-large">Create Shareable</p>
                <form asp-action="CreateShareable" asp-controller="Shareable" enctype="multipart/form-data">
                    <div asp-validation-summary="ModelOnly"></div>
                    <div className="field-group field-group--medium">
                        <label asp-for="Title" className="field-label field-label--medium"></label>
                        <div className="input-wrapper-medium">
                            <input asp-for="Title" className="field-input field-input--title" />
                        </div>
                        <span asp-validation-for="Title"></span>
                    </div>
                    <div className="field-group field-group--medium">
                        <label asp-for="Introduction" className="field-label field-label--medium"></label>
                        <div className="input-wrapper-medium">
                            <textarea rows="4" cols="50" asp-for="Introduction" className="field-input field-input--area"></textarea>
                        </div>
                    </div>

                    <div className="field-group field-group--medium">
                        <label for="Paragraphs[0].Text" className="field-label field-label--medium">Paragraphs</label>
                        <btn id="btn_add" className="btn btn-icon" style="margin:0 0.5rem">+</btn><btn id="btn_remove" className="btn btn-icon">-</btn>
                        <div id="paragraphs_container" className="input-wrapper-medium">
                            <textarea rows="10" cols="50" id="Paragraphs_0__Text" name="Paragraphs[0].Text" className="field-input field-input--area froala"></textarea>
                        </div>
                    </div>
                    <div className="field-group field-group--medium">
                        <input type="file" id="input_image" name="avatar" hidden />

                        <button id="btn_browse" type="button" className="btn">Select Display Image</button>

                        <div id="div_preview" className="hidden-element" style="margin-top:1.5rem;">
                            <p id="p_filename" className="field-label field-label--medium"></p>
                        </div>
                    </div>

                    <div className="form-actions" style="margin-top:5rem;">
                        <button className="btn btn--large1">Submit</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default ManageShareable;