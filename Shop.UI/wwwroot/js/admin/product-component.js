Vue.component('product-manager', {
    template: `<div v-if="!editing">
                <button class="button" @click="newProduct">Add new product</button>
                <table class="table">
                    <tr>
                        <th>
                            Id
                        </th>
                        <th>
                            Name
                        </th>
                        <th>
                            Value
                        </th>
                        <th></th>
                        <th></th>
                    </tr>
                    <tr v-for="(product, index) in products">
                        <td>
                            {{product.id}}
                        </td>
                        <td>
                            {{product.name}}
                        </td>
                        <td>
                            {{product.value}}
                        </td>
                        <td>
                            <button class="linkLike" @click="editProduct(product.id, index)">Edit</button>
                        </td>
                        <td>
                            <button class="linkLike" @click="deleteProduct(product.id, index)">Delete</button>
                        </td>
                    </tr>
                </table>
            </div>         
            <div v-else>
                <div class="field">
                    <label class="label">Product Name</label>
                    <div class="control">
                        <input class="input" v-model="productModel.name" />
                    </div>
                </div>
                <div class="field">
                    <label class="label">Product Description</label>
                    <div class="control">
                        <input class="input" v-model="productModel.description" />
                    </div>
                </div>
                <div class="field">
                    <label class="label">Product Value</label>
                    <div class="control">
                        <input class="input" v-model="productModel.value" />
                    </div>
                </div>

                <button class="button is-success" @click="createProduct" v-if="!productModel.id">Create product</button>
                <button class="button is-warning" @click="updateProduct" v-else>Update product</button>
                <button class="button is-delete" @click="cancelEditing">Cancel</button>
            </div>`,
    data() {
        return {
            loading: false,
            editing: false,
            products: [],
            objectIndex: 0,
            productModel: {
                id: 0,
                name: "Product name",
                description: "Product description",
                value: "0.01"
            }
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
                this.editing = false;
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
                this.editing = false;
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
        this.editing = true;
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
                this.editing = false;
            });
    },

    deleteProduct(id, index) {
        axios.delete("/Admin/products/" + id)
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
    },
    cancelEditing() {
        this.editing = false;
    },
    newProduct() {
        this.editing = true;
        this.productModel.id = 0;
    }
},
    computed: {
        formatPrice: function () {
            return "\u20bd" + this.price;
        }
    }
})