class SubscriberList {
    constructor() {
        const activeElement = document.getElementById("Active");
        const inactiveElement = document.getElementById("InActive");

        if (activeElement && inactiveElement) {
            activeElement.addEventListener("change", () => {
                this.checkboxState(activeElement, inactiveElement);
            });
        
            inactiveElement.addEventListener("change", () => {
                this.checkboxState(activeElement, inactiveElement);
            });
        }
    }

    checkboxState(activeElement, inactiveElement) {
        if (inactiveElement.checked && !activeElement.checked) {
            this.callController("false");
            return;
        }
        if (activeElement.checked && inactiveElement.checked) {
            this.callController("both");
            return;
        }
        if (activeElement.checked && !inactiveElement.checked) {
            this.callController("true");
            return;
        }

        this.callController(null);
    }

    callController(active) {
        window.location.href = `/eiir/Admin/Subscribers/1/${active}`;
    }
}

export const subscriberList = new SubscriberList();