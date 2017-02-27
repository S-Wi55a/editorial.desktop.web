(function (windowP, documentP, fpcDomain) {
    var that = this;
    var ctx = {};
    var lTrack = '';

    windowP = windowP || window;
    documentP = documentP || document;

    var getUrlParam = function (name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(windowP.location.href);
        if (results == null)
            return "";
        else
            return results[1];
    };

    var trim = function (val) {
        return val.replace(/^\s+|\s+$/g, '');
    };

    var setCookie = function (cookieName, value, time) {
        var expires = '', domain = '';
        if (time) {
            expires = ";expires=" + time.toGMTString();
        }
        if (fpcDomain) {
            domain = ';domain=' + windowP;
        }
        documentP.cookie = cookieName + "=" + value + expires + domain + ';path=/';
    };

    var getCookie = function (cookieName) {
        var ucookies = documentP.cookie.split(';'),
           cookieVal = '',
           np, nc,
           i, ucl = ucookies.length;

        if (documentP.cookie != "") {
            for (i = 0; i < ucl; i++) {
                np = ucookies[i].split("=");
                nc = trim(np[0]);
                if (nc == cookieName) {
                    cookieVal = decodeURIComponent(trim(np.splice(1, np.length - 1).join('=')));
                }
            }
        }
        return cookieVal;
    };

    var getTime = function (seconds) {
        var t = new Date();
        t.setTime(t.getTime() + (seconds * 1000));
        return t;
    };

    that.doGenCookieUrl = function (urlP, cName, time) {
        var cdxP = getUrlParam(urlP) || getCookie(cName);
        if (cdxP) {
            setCookie(cName, cdxP, time);
            return cdxP;
        }
        return '';
    };

    that.doCdxWt = function () {
        return that.doGenCookieUrl('CDX.WT', 'WT_FPC', getTime(60 * 60 * 24 * 365 * 10));
    };

    that.doCdxMs = function () {
        return that.doGenCookieUrl('CDX.MS', 'WT_MS', getTime(60 * 60 * 24 * 30));
    };

    that.doAttrC = function () {
        return that.doGenCookieUrl('CDX.AC', 'WT_AC', getTime(60 * 60 * 24 * 30));
    };

    that.doAttrL = function () {
        return that.doGenCookieUrl('CDX.LA', 'WT_LA', getTime(60 * 60 * 24 * 30));
    };

    that.doAttrF = function () {
        return that.doGenCookieUrl('CDX.FA', 'WT_FA', getTime(60 * 60 * 24 * 30));
    };

    that.getWTUrlParam = function () {
        var cVal = getCookie('WT_FPC'),
            acVal = getCookie('WT_AC'),
            alVal = getCookie('WT_LA'),
            afVal = getCookie('WT_FA');

        var query = that.queryBuilder('/');

        if (cVal) {
            query.update('CDX.WT', encodeURIComponent(cVal));
        }
        if (acVal) {
            query.update('CDX.AC', encodeURIComponent(acVal));
        }
        if (alVal) {
            query.update('CDX.LA', encodeURIComponent(alVal));
        }
        if (afVal) {
            query.update('CDX.FA', encodeURIComponent(afVal));
        }
        lTrack = query.toString();
        return lTrack;
    };

    that.queryBuilder = function (url, noDecode) {
        var list = {},
            queryString = (url.match(/\?([^#]+)#?/) ? RegExp.$1 : ""),
            other = url.replace(queryString, ""),
            params = queryString.split("&");

        $.each(params, function () {
            var keyValue = this.split("=");
            list[keyValue[0]] = !noDecode ? decodeURI(keyValue[1]) : keyValue[1];
        });

        return {
            update: function (key, value) {
                list[key] = value;
            },
            remove: function (key) {
                delete list[key];
            },
            value: function (key) {
                return list[key] || "";
            },
            toString: function (all) {
                var result = [];
                for (var key in list) {
                    if (key && list.hasOwnProperty(key)) {
                        result.push(key + "=" + $.trim(list[key]));
                    }
                }
                if (all) {
                    return ((other.indexOf('?') < 0) ? other + '?' : other) + result.join("&");
                } else {
                    return result.join("&");
                }
            }
        };
    };

    that.wireUpCrossDomainLinks = function () {
        $('a[data-webm-cdlink]').each(function (idx) {
            var href = $(this).attr('href'),
                qb = that.queryBuilder(href, true),
                cVal = getCookie('WT_FPC'),
                mVal = getCookie('WT_MS'),
                acVal = getCookie('WT_AC'),
                alVal = getCookie('WT_LA'),
                afVal = getCookie('WT_FA');

            if (cVal) {
                qb.update('CDX.WT', encodeURIComponent(cVal));
            }
            if (mVal) {
                qb.update('CDX.MS', encodeURIComponent(mVal));
            }
            if (acVal) {
                qb.update('CDX.AC', encodeURIComponent(acVal));
            }
            if (alVal) {
                qb.update('CDX.LA', encodeURIComponent(alVal));
            }
            if (afVal) {
                qb.update('CDX.FA', encodeURIComponent(afVal));
            }
            $(this).attr('href', qb.toString(true));
        });

    };

    try {
        var search = windowP.location.search;
        ctx.mId = that.doCdxMs();
        ctx.wtId = that.doCdxWt();
        ctx.attrC = that.doAttrC();
        ctx.attrL = that.doAttrL();
        ctx.attrF = that.doAttrF();
        if (search.indexOf('CDX.MS') > -1 ||
            search.indexOf('CDX.WT') > -1 ||
            search.indexOf('CDX.AC') > -1 ||
            search.indexOf('CDX.LA') > -1 ||
            search.indexOf('CDX.FA') > -1) {
            ctx.cdxOccurred = true;
            var qb = that.queryBuilder(search);
            qb.remove('CDX.MS');
            qb.remove('CDX.WT');
            qb.remove('CDX.AC');
            qb.remove('CDX.LA');
            qb.remove('CDX.FA');
            var stateR = windowP.location.pathname;
            if (qb.toString().length > 0) {
                stateR += '?' + qb.toString();
            }
            if (windowP.location.hash) {
                stateR += windowP.location.hash;
            }

            windowP.history.replaceState("", "", stateR);
        }
        that.getWTUrlParam();
        that.wireUpCrossDomainLinks();
    } catch (e) {

    };

    return {
        context: ctx,
        debug: that,
        linkTrackParam: lTrack
    };
})(window, document, (function () {
    var x = window.location.host.split('.');
    return '.' + x.splice(1, x.length - 1).join('.');
})());