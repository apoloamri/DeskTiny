new Vue({
    el: '#member_register',

    data: {
        username: "",
        password: "",
        confirm_password: "",
        email: "",
        first_name: "",
        last_name: "",
        messages: {}
    },

    methods: {
        Register: function () {
            var that = this;
            $.post(apiUrl + "member/register", {
                "username": that.username,
                "password": that.password,
                "confirm_password": that.confirm_password,
                "email": that.email,
                "first_name": that.first_name,
                "last_name": that.last_name
            },
            function (data) {
                if (data.is_valid) {
                    alert("Successfully registered.");
                    window.location = "index.html";
                }
                else {
                    that.messages = data.messages;
                }
            });
        }
    }
});