class FeedbackList {
    constructor() {
        const insolvencyElement = document.getElementById("type");
        const organisationElement = document.getElementById("organisation");
        const statusElement = document.getElementById("status");

        if (insolvencyElement) {
            insolvencyElement.addEventListener("change", () => {
                this.filterChanged(insolvencyElement, organisationElement, statusElement);
            });
        }
        if (organisationElement) {
            organisationElement.addEventListener("change", () => {
                this.filterChanged(insolvencyElement, organisationElement, statusElement);
            });
        }
        if (statusElement) {
            statusElement.addEventListener("change", () => {
                this.filterChanged(insolvencyElement, organisationElement, statusElement);
            });
        }
    }

    filterChanged(insolvencyElement, organisationElement, statusElement) {
        var insolvency = insolvencyElement.value;
        var organisation = organisationElement.value;
        var status = statusElement.value;

        this.callController(insolvency, organisation, status);
    }

    callController(insolvency, organisation, status) {
        window.location.href = `/admin/errors-or-issues/1/${insolvency}/${organisation}/${status}`;
    }
}

export const feedbackList = new FeedbackList();