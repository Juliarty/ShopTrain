var app = new Vue({
    el: "#app",
    data: {
        loading: false,
    },
    methods: {
        createSession() {
            axios.post("/Checkout/Payment/create-session")
                .then(response => {
                    return response.json();
                })
                .then(session => {
                    return stripe.redirectToCheckout({ sessionId: session.id });
                })
                .then(result => {
                    // If redirectToCheckout fails due to a browser or network
                    // error, you should display the localized error message to your
                    // customer using error.message.
                    if (result.error) {
                        alert(result.error.message);
                    }
                })
                .catch(error => {console.error("Error:", error);});
        }
    }
});
