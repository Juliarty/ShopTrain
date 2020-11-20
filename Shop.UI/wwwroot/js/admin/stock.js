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
        selectProduct(product) {
            this.selectedProduct = product;
            this.newStock.productId = product.id;
        },
        getStock() {
            this.loading = true;
            axios.get("/Admin/stocks")
                .then(res => {
                    console.log(res.data);
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        addStock() {
            this.loading = true;
            axios.post("/Admin/stocks", this.newStock)
                .then(res => {
                    console.log(res);
                    this.selectedProduct.stock.push(res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                    this.editing = false;
                });
        },
        updateStock() {
            this.loading = true;
            var updatedProduct = this.selectedProduct;

            var stock = updatedProduct.stock.map(x => {
                return {
                    id: x.id,
                    description: x.description,
                    qty: x.qty,
                    productId: updatedProduct.id
                };
            });
            var requestData = { stock: stock };
            axios.put("/Admin/stocks/", requestData)
                .then(res => {
                    console.log(res.data);
                    updatedProduct.stock = res.data.stock;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        deleteStock(stock, index) {
            var currentProduct = this.selectedProduct;
            axios.delete("/Admin/stocks/" + stock.id)
                .then(res => {
                    console.log(res);
                    currentProduct.stock.splice(index, 1);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        }
    }
});