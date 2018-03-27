new Vue({
    el: '#login',

    data: {
        username: "",
        password: "",
    },

    methods: {
        Login: function () {
            var that = this;
            $.post(apiUrl + "login", {
                "username": that.username,
                "password": that.password
            },
            function (data) {
                if (data.is_valid) {
                    setCookie("session_id", that.username, 1);
                    setCookie("session_key", data.session_key, 1);
                    window.location = "home.html";
                }
                else {
                    alert("Invalid username or password.");
                }
            });
        }
    }
});