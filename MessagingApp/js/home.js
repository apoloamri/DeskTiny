new Vue({
    el: '#messenger',

    data: {
        search: "",
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

        Search: function () {
            var that = this;
            $.ajax({
                url: apiUrl + "messenger/search",
                type: "GET",
                dataType: "json",
                data: {
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
        }
    },

    created: function () {
        checkLogin(false);
    },

    // watch: {
    //     username: function() {
    //         this.Search();
    //     },
    //     email: function() {
    //         this.Search();
    //     }
    // }
});