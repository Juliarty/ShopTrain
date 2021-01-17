var app = new Vue({
    el: "#app",
    data: {
        loading: false,
        status: 0,
        orders: [],
        selectedOrder: null
        
    },
    mounted() {
        this.getOrders();
    },
    watch: {
        status: function () {
            this.getOrders()
        }
    },
    methods: {
        getOrders() {
            this.loading = true;
            console.log("getOrders()");
            axios.get("/orders?status=" + this.status)
                .then(res => {
                    this.orders = res.data.orders;
                    console.log(this.orders.length);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        selectOrder(id) {
            this.loading = true;
            axios.get("/orders/" + id)
                .then(res => {
                    console.log(res);
                    this.selectedOrder = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        updateOrder() {
            this.loading = true;
            axios.put("/orders/" + this.selectedOrder.id, null)
                .then(res => {
                    console.log(res);
                    this.exitOrder();
                    this.getOrders();
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        exitOrder() {
            this.selectedOrder = null;
        }


    },
    computed: {
    }
});

