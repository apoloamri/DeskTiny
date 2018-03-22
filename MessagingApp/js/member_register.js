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
            $.ajax({
                url: apiUrl + "member/register",
                type: "POST",
                dataType: "json",
                data: {
                    "username": that.username,
                    "password": that.password,
                    "confirm_password": that.confirm_password,
                    "email": that.email,
                    "first_name": that.first_name,
                    "last_name": that.last_name,
                    "messages": that.messages
                },
                success: function (data) {
                    if (data.is_valid) {
                        alert("Successfully registered.");
                        window.location = "index.html";
                    }
                    else {
                        that.messages = data.messages;
                    }
                }
            });
        }
    },

    created: function () {
        //checkLogin(false);
    }
});