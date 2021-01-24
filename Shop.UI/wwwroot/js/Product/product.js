var app = new Vue({
    el: "#app",
    data: {
        productId: 0,
        loading: false,
        stockModel: {
            id: 0,
            qty: -1
        }
    },
    methods: {
        onChangeSelectHandler() {
            // get current stock id
            var select = document.getElementById("selectQty");
            var id = select.options[select.selectedIndex].value;
            //get number of products in the selected stock
            this.getStockQty(id);
        },
        getStockQty(stockId) {
            this.loading = true;
            axios.get("/Product/stocks", { params: { productId: this.productId, stockId: stockId } })
                .then(res => {
                    this.stockModel.id = stockId;
                    this.stockModel.qty = res.data
                    console.log("ptoductId: " + this.productId + " stockId: " + stockId + " qty: " + res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        }

        

    },
    mounted() {
        // get product id
        var productId = document.getElementById("productId");
        this.productId = productId.value;
        //
        this.onChangeSelectHandler();
    }
});

