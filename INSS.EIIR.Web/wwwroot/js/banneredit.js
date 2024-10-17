class BannerEdit {
    constructor() {
        const previewElement = document.getElementById("bannerPreview");
        const nopreviewElement = document.getElementById("bannerNoPreview");
        const bannertextElement = document.getElementById("BannerText");
        const bannerpreviewtextElement = document.getElementById("BannerPreviewText");


        if (bannertextElement) {
            bannertextElement.addEventListener("keyup", () => {
                this.textChanged(bannertextElement, previewElement, nopreviewElement, bannerpreviewtextElement);
            });
        }
    }

    textChanged(bannertextElement, previewElement, nopreviewElement, bannerpreviewtextElement) {
        if (bannertextElement.value != null && bannertextElement.value.trim() !== '')
        {
            previewElement.className = "govuk-!-display-inline"; 
            nopreviewElement.className = "govuk-!-display-none";
            bannerpreviewtextElement.textContent = bannertextElement.value;
        }
        else
        {
            previewElement.className = "govuk-!-display-none";
            nopreviewElement.className = "govuk-!-display-inline";
        }
    }
}

export const bannerEdit = new BannerEdit();