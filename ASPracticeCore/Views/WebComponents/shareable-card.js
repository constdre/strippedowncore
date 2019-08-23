class ShareableCard extends HTMLElement {
    constructor() {
        let template = document.getElementById("template_shareable_item");
        let templateNode = template.content.cloneNode(true);
        const shadowRoot = this.attachShadow({ mode: 'open' });
        shadowRoot.appendChild(templateNode);
    }
}
customElements.define("shareable-card", ShareableCard);