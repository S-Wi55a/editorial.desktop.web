(function (wn) {
    "use strict";

    wn.KruxSasHelper = function () {
        var retrieve = function (n) {
            var m, k = "kx" + n;
            if (window.localStorage) {
                return window.localStorage[k] || "";
            } else if (navigator.cookieEnabled) {
                m = document.cookie.match(k + "=([^;]*)");
                return (m && unescape(m[1])) || "";
            } else {
                return "";
            }
        }

        var getSasData = function () {
            var user = retrieve('user');
            var segments = retrieve('segs');

            var ksg = "";

            if (segments) {
                ksg = segments;
            }

            return {
                kuid: user,
                ksg: ksg
            }
        }

        var getSasTags = function () {
            var user = retrieve("user");
            var segments = retrieve("segs") ? retrieve("segs").split(",") : "";

            var kvs = [];
            if (user) {
                kvs.push("kuid=" + user);
            }

            if (segments) {
                kvs.push("ksg=" + segments.join(","));
            }

            return kvs.length ? kvs.join("/") : "";
        };

        return {
            getSasTags: getSasTags,
            getSasData: getSasData
        };
    };

})(window);