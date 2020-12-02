var app = new Vue({
    el: "#app",
    data: {
        loading: false,
        newStock: {
            productId: 0,
            description: "Size",
            qty: 10
        },
        selectedProduct: null,
        products: null
    },
    mounted() {
        this.getStock();
    },
    methods: {
        createSession(stripe) {
            axios.post("/Payment")
                .then(x => x.json())
                .then(session => {
                    stripe.redirectToCheckout({ sessionId: session.id })
                })
                .then(result => {
                    // If redirectToCheckout fails due to a browser or network
                    // error, you should display the localized error message to your
                    // customer using error.message.
                    if (result.error) {
                        alert(result.error.message);
                    }
                })
                .catch(error => {
                    console.error("Error:", error);
                });
        }
    }
});
