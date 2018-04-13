new Vue({
    el: '#messenger',

    data: {
        count: 10,
        search: "",
        message: "",
        username: "",
        messages: [],
        contacts: [],
        result: [],
        uncontacts: [],
        information: {},

        group_id: null,
        group_name: "",
        members: "",
        groups: []
    },

    methods: {
        GetInformation: function () {
            var that = this;
            $.get(apiUrl + "member/info", {
                "session_id": getCookie("session_id"),
                "session_key": getCookie("session_key")
            },
            function (data) {
                if (data.is_valid) {
                    that.information = data.result;
                }
            });
        },

        NextMessages: function () {
            var that = this;
            that.count += 10;
        },

//Messanging - start
        Search: function () {
            var that = this;
            $.get(apiUrl + "messenger/search", {
                "session_id": getCookie("session_id"),
                "session_key": getCookie("session_key"),
                "username": that.search,
                "email": that.search
            },
            function (data) {
                if (data.is_valid) {
                    that.result = data.result;
                    that.search = "";
                }
            });
        },

        AddContact: function (username) {
            var that = this;
            $.post(apiUrl + "messenger/add", {
                "session_id": getCookie("session_id"),
                "session_key": getCookie("session_key"),
                "username": username
            },
            function (data) {
                if (data.is_valid) {
                    alert("Contact added.");
                }
                else {
                    alert(data.messages.username);
                }
            });
        },

        GetContacts: function () {
            var that = this;
            $.get(apiUrl + "member/contacts", {
                "session_id": getCookie("session_id"),
                "session_key": getCookie("session_key")
            },
            function (data) {
                if (data.is_valid) {
                    that.contacts = data.result;
                    that.uncontacts = data.unaccepted_results;
                    //setTimeout(function () { that.GetContacts() }, 2000);
                }
            });
        },

        SelectRecipient: function (username) {
            var that = this;
            that.group_id = null;
            that.username = username;
            that.count = 10;
            that.ShowMessages();
        },

        ShowMessages: function () {
            var that = this;
            if (that.username == "") {
                return;
            }
            $.get(apiUrl + "messenger/message", {
                "session_id": getCookie("session_id"),
                "session_key": getCookie("session_key"),
                "username": that.username,
                "count": that.count
            },
            function (data) {
                if (data.is_valid) {
                    that.messages = data.result;
                    //setTimeout(function () { that.ShowMessages() }, 1000);
                }
            });
        },

        SendMessage: function () {
            var that = this;
            $.post(apiUrl + "messenger/message", {
                "session_id": getCookie("session_id"),
                "session_key": getCookie("session_key"),
                "username": that.username,
                "message": that.message
            },
            function (data) {
                if (data.is_valid) {
                    that.message = "";
                }
            });
        },
//Messanging - end

//Grouping - start   
        AddGroup: function () {
            var that = this;
            var members = that.members.replace(/ /g,'');
            $.post(apiUrl + "messenger/group/create", {
                "session_id": getCookie("session_id"),
                "session_key": getCookie("session_key"),
                "group_name": that.group_name,
                "members": members.split(",")
            },
            function (data) {
                if (data.is_valid) {
                    alert("Group created.");
                    hat.group_name = "";
                    that.members = "";
                }
                else {
                    that.messages = data.messages;
                }
            });
        },

        GetGroups: function () {
            var that = this;
            $.get(apiUrl + "messenger/group", {
                "session_id": getCookie("session_id"),
                "session_key": getCookie("session_key")
            },
            function (data) {
                if (data.is_valid) {
                    that.groups = data.result;
                }
            });
        },

        SelectGroup: function (id) {
            var that = this;
            that.username = "";
            that.group_id = id;
            that.count = 10;
            that.ShowGroupMessages();
        },

        ShowGroupMessages: function () {
            var that = this;
            if (that.group_id == null) {
                return;
            }
            $.get(apiUrl + "messenger/group", {
                "session_id": getCookie("session_id"),
                "session_key": getCookie("session_key"),
                "group_id": that.group_id,
                "count": that.count
            },
            function (data) {
                if (data.is_valid) {
                    that.messages = data.result;
                    //setTimeout(function () { that.ShowGroupMessages() }, 1000);
                }
            });
        },

        SendGroupMessage: function () {
            var that = this;
            $.post(apiUrl + "messenger/group", {
                "session_id": getCookie("session_id"),
                "session_key": getCookie("session_key"),
                "group_id": that.group_id,
                "message": that.message
            },
            function (data) {
                if (data.is_valid) {
                    that.message = "";
                }
            });
        },
//Grouping - end
    },

    created: function () {
        this.GetContacts();
        this.GetGroups();
        this.GetInformation();
    }
});