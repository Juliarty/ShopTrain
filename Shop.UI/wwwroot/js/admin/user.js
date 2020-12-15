var app = new Vue({
    el: "#app",
    data: {
        loading: false,
        username: ""
    },
    mounted() {
        //ToDo: get all users
    },
    methods: {

        createUser() {
            this.loading = true;
            axios.post("/users", { username: this.username })
                .then(res => {
                    console.log(res.data);
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