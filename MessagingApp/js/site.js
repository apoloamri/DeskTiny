// Write your Javascript code.
var apiUrl = "http://localhost:60400/";
var siteActive = false;

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function checkLogin(logInPage) {
    var username = getCookie("session_id");
    var session_key = getCookie("session_key");
    $.get(apiUrl + "login", {
        "username": username,
        "session_key": session_key
    },
    function (data) {
        var logged_in = data.logged_in;
        if (logInPage) {
            if (logged_in) {
                window.location = "home.html";
            }
        }
        else {
            if (!logged_in) {
                window.location = "index.html";
            }
        }
    });
}

function logout() {
    setCookie("username", "", 0);
    setCookie("session_key", "", 0);
    window.location = "index.html";
}

function activeTimeout(func, timeout) {
    if (siteActive) {
        setTimeout(func, timeout);
    }
}

// function showLoading() {
//     $("#loading").addClass("show");
//     $("#loading-background").addClass("show");
// }

// function hideLoading() {
//     $("#loading").removeClass("show");
//     $("#loading-background").removeClass("show");
// }
