﻿var app = new Vue({
    el: "#app",
    data: {
        loading: false,
        products: [],
        objectIndex: 0,
        productModel: {
            id: 0,
            name: "Product name",
            description: "Product description",
            value: "0.01"
        }
    },
    mounted() {
        this.getProducts();
    },
    methods: {
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
            axios.post("/Admin/products", this.productModel)
                .then(res => {
                    console.log(res);
                    this.products.push(res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },

        getProduct(id) {
            axios.get("/Admin/products/" + id)
                .then(res => {
                    console.log(res);
                    var product = res.data;

                    this.productModel = {
                        id: product.id,
                        name: product.name,
                        description: product.description,
                        value: product.value
                    };
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        editProduct(id, index) {
            this.objectIndex = index;
            this.getProduct(id);

        },
        updateProduct() {
            axios.put("/Admin/products/", this.productModel)
                .then(res => {
                    console.log(res);
                    this.products.splice(this.objectIndex, 1, res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },

        deleteProduct(id, index) {
            axios.delete("/Admin/products/" +  id)
                .then(res => {
                    console.log(res);
                    this.products.splice(index, 1);
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