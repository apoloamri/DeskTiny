new Vue({
    el: '#messenger',

    data: {
        search: "",
        messages: [],
        contacts: [],
        result: []
    },

    methods: {
        Search: function () {
            var that = this;
            $.ajax({
                url: apiUrl + "messenger/search",
                type: "GET",
                dataType: "json",
                data: {
                    "session_id": getCookie("session_id"),
                    "session_key": getCookie("session_key"),
                    "username": that.search,
                    "email": that.search
                },
                success: function (data) {
                    if (data.is_valid) {
                        that.result = data.result;
                    }
                    else {
                        
                    }
                }
            });
        },

        AddContact: function (username) {
            var that = this;
            $.ajax({
                url: apiUrl + "messenger/add",
                type: "POST",
                dataType: "json",
                data: {
                    "session_id": getCookie("session_id"),
                    "session_key": getCookie("session_key"),
                    "username": username
                },
                success: function (data) {
                    if (data.is_valid) {
                        this.GetContacts();
                        alert("Contact added.");
                    }
                    else {
                        alert(data.messages.username);
                    }
                }
            });
        },

        GetContacts: function () {
            var that = this;
            $.ajax({
                url: apiUrl + "member/contacts",
                type: "GET",
                dataType: "json",
                data: {
                    "session_id": getCookie("session_id"),
                    "session_key": getCookie("session_key")
                },
                success: function (data) {
                    if (data.is_valid) {
                        that.contacts = data.result;
                    }
                    else {
                        
                    }
                }
            });
        },

        ShowMessages: function (username) {
            var that = this;
            $.ajax({
                url: apiUrl + "messenger/message",
                type: "GET",
                dataType: "json",
                data: {
                    "session_id": getCookie("session_id"),
                    "session_key": getCookie("session_key"),
                    "username": username
                },
                success: function (data) {
                    if (data.is_valid) {
                        that.messages = data.result;
                    }
                    else {
                        
                    }
                }
            });
        }
    },

    created: function () {
        this.GetContacts();
    }

    // watch: {
    //     username: function() {
    //         this.Search();
    //     },
    //     email: function() {
    //         this.Search();
    //     }
    // }
});