var app = new Vue({
    el: "#app",
    data: {
        price: 0,
        showPrice: true,
        loading: false,
        products: [],

        productModel: {
            name: "Product name",
            description: "Product description",
            value: 0.01
        }
    },
    methods: {
        togglePrice: function () {
            this.showPrice = !this.showPrice;
        },
        alert(v) {
            alert(v);
        },

        getProducts() {
            this.loading = true;
            axios.get("/Admin/products")
                .then(res => {
                    console.log(res);
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },

        createProduct() {
            this.loading = true;
            axios.post("/Admin/product", this.productModel)
                .then(res => {
                    console.log(res);
                    
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        }
    },
    computed: {
        formatPrice: function () {
            return "\u20bd" + this.price;
        }
    }

});