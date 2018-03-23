new Vue({
    el: '#messenger',

    data: {
        search: "",
        message: "",
        username: "",
        messages: [],
        contacts: [],
        result: [],
        uncontacts: [],
        information: {}
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
                        that.GetContacts();
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
                        that.uncontacts = data.unaccepted_results
                    }
                    else {
                        
                    }
                }
            });
        },

        SelectRecipient: function (username) {
            var that = this;
            that.username = username;
            that.ShowMessages();
        },

        ShowMessages: function () {
            var that = this;
            $.ajax({
                url: apiUrl + "messenger/message",
                type: "GET",
                dataType: "json",
                data: {
                    "session_id": getCookie("session_id"),
                    "session_key": getCookie("session_key"),
                    "username": that.username
                },
                success: function (data) {
                    if (data.is_valid) {
                        that.messages = data.result;
                        setTimeout(function () { that.ShowMessages(that.username) }, 2000);
                    }
                    else {
                        
                    }
                }
            });
        },

        SendMessage: function () {
            var that = this;
            $.ajax({
                url: apiUrl + "messenger/message",
                type: "POST",
                dataType: "json",
                data: {
                    "session_id": getCookie("session_id"),
                    "session_key": getCookie("session_key"),
                    "username": that.username,
                    "message": that.message
                },
                success: function (data) {
                    if (data.is_valid) {
                        
                    }
                    else {
                        
                    }
                }
            });
        },

        GetInformation: function () {
            var that = this;
            $.ajax({
                url: apiUrl + "member/info",
                type: "GET",
                dataType: "json",
                data: {
                    "session_id": getCookie("session_id"),
                    "session_key": getCookie("session_key")
                },
                success: function (data) {
                    if (data.is_valid) {
                        that.information = data.result;
                    }
                    else {
                        
                    }
                }
            });
        }
    },

    created: function () {
        this.GetContacts();
        this.GetInformation();
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