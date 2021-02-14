var app = new Vue({
    el: "#app",
    data: {
        loading: false,
        username: "",
        password: ""
    },
    mounted() {
        //ToDo: get all users
    },
    methods: {

        createUser() {
            this.loading = true;

            axios.post("/users", { username: this.username, password: this.password })
                .then(res => {
                    console.log(res.data);

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