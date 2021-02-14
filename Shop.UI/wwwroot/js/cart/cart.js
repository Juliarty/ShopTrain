var app = new Vue({
    el: "#app",
    data: {
        loading: false,
        cartItems: []
    },
    mounted() {
        this.getCart();
    },
    methods: {

        getCart() {
            this.loading = true;
            axios.get("Cart/items")
                .then(res => {
                    this.cartItems = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },

        addOneToCart(e) {
            var stockId = e.target.dataset.stockId;
            this.loading = true;

            axios.post("Cart/AddOne/" + stockId, null)
                .then(res => {
                    this.getCart();
                    this.updateSmallCart();
                })
                .catch(err => {
                    console.log(err);
                    if (err.response) {
                        console.log(err.response.data);
                    }
                })
                .then(() => {
                    this.loading = false;
                });
        },

        removeOneFromCart(e) {
            var stockId = e.target.dataset.stockId;
            this.loading = true;

            axios.post("Cart/SubOne/" + stockId, null)
                .then(res => {
                    this.getCart();
                    this.updateSmallCart();
                })
                .catch(err => {
                    console.log(err);
                    if (err.response) {
                        console.log(err.response.data);
                    }
                })
                .then(() => {
                    this.loading = false;
                });

        },

        removeItem(e) {
            var stockId = e.target.dataset.stockId;
            this.loading = true;

            axios.post("Cart/RemoveItem/" + stockId, null)
                .then(res => {
                    this.cartItems = [];
                    this.getCart();
                    this.updateSmallCart();

                })
                .catch(err => {
                    console.log(err);
                    if (err.response) {
                        console.log(err.response.data);
                    }
                })
                .then(() => {
                    this.loading = false;
                });

        },
        updateSmallCart() {
            this.loading = true;

            axios.get("Cart/GetSmallCart")
                .then(res => {
                    var el = document.getElementById("nav-small-cart");
                    el.outerHTML = res.data;
                })
                .catch(err => {
                    console.log(err);
                    if (err.response) {
                        console.log(err.response.data);
                    }
                })
                .then(() => {
                    this.loading = false;
                });
        }
    }
});


