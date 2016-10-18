function TrafficSourceAttribution(isBrandRelatedCallBack, tag, windowP, documentP) {
    var that = this;
    var domain = '';
    if (tag) {
        domain = ';domain=' + tag.fpcdom;
    }

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

    that.rhost = documentP.location.host;
    that.ref = documentP.referrer;
    var testRef = getUrlParam("testref");
    if (testRef)
        that.ref = decodeURIComponent(testRef);

    var ctx = {};
    var attributions = {};
    var sessionCount = 0;

    var tsMap = [
        'Direct to site',
        'Paid search - Brand',
        'Paid search - Generic',
        'Organic search - Brand',
        'Organic search - Generic',
        'Social - Earned',
        'Carsales Network',
        'Other',
        'Email - Newsletter',
        'Email - Transactional',
        'Display',
        'Social - Content'
    ];
    var csNetwork = [
        'bikesales',
        'boatsales',
        'carsales',
        'caravancampingsales',
        'quicksales',
        'redbook',
        'motoring',
        'trucksales',
        'carpoint',
        'bikepoint',
        'boatpoint',
        'farmmachinerysales',
        'constructionsales',
        'livemarket',
        'discountnewcars',
        'homesales'
    ];
    var pCom = [
        'AutoAlert',
        'PriceAlert',
        'PCAA',
        'PCPA'
    ];

    var trim = function (val) {
        return val.replace(/^\s+|\s+$/g, '');
    };

    var getRefDomain = function () {
        var rs = that.ref.split('/');
        if (rs.length >= 3)
            return rs[2].toLowerCase();
        return null;
    };

    var loadAttributions = function () {
        var cc = getCookie('WT_AC'),
            at,
            av,
            ac;

        sessionCount = 0;
        if (cc != '') {
            at = cc.split("|");
            for (var i = 0, atl = at.length; i < atl; i++) {
                av = at[i].split('.');
                ac = Number(av[1]);
                attributions[av[0]] = ac;
                sessionCount += ac;
            }
        }
    };

    var saveAttributions = function () {
        var attr = [], v;
        for (var at in attributions) {
            attr.push(at + "." + attributions[at]);
        }
        v = attr.join("|");
        setCookie('WT_AC', v, that.cookieTime);
    };

    var getHighestAttribution = function () {
        var ha = 0, hc = 0;
        for (var at in attributions) {
            if (attributions[at] > hc) {
                hc = attributions[at];
                ha = at;
            }
        }
        return ha;
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

    var setCookie = function (cookieName, value, time) {
        var expires = '';
        if (time) {
            expires = ";expires=" + time.toGMTString();
        }
        documentP.cookie = cookieName + "=" + value + expires + domain + ';path=/';
    };

    var getTime = function (seconds) {
        var t = new Date();
        var adj = (t.getTimezoneOffset() * 60000) + (10 * 3600000);
        t.setTime(t.getTime() + adj + (seconds * 1000));
        return t;
    };

    that.isSocial = function () {
        var social = ['facebook', 'twitter', 'youtube', 'bit'];
        if (!that.referringDomain)
            return false;

        for (var i = 0, sl = social.length; i < sl; i++) {
            if (that.referringDomain.indexOf(social[i]) >= 0)
                return true;
        }
        return false;
    };

    that.isSearch = function () {
        return (
		   (that.ref.indexOf('.google.') > 0)
		   || (that.ref.indexOf('.bing.') > 0 && that.ref.indexOf('?q=') > 0)
		   || (that.ref.indexOf('.yahoo.') > 0 && that.ref.indexOf('p=') > 0)
		   || (that.ref.indexOf('.baidu.') > 0 && that.ref.indexOf('?wd=') > 0)
		   || (that.ref.indexOf('.search.com') > 0 && that.ref.indexOf('?q=') > 0)
		   || (that.ref.indexOf('.aol.') > 0 && that.ref.indexOf('query=') > 0)
		   || (that.ref.indexOf('.ask.') > 0 && that.ref.indexOf('?q=') > 0)
		);
    };

    that.isPaid = function () {
        if (documentP.location || documentP.location.search) {
            var search = documentP.location.search.toLowerCase();
            return (
                (search.indexOf('gclid') > 0)
                || (search.indexOf('utm_source') > 0)
            );
        }
        return false;
    };


    that.isCsNetwork = function () {
        var rd = that.referringDomain;
        if (that.rhost.indexOf(rd) == -1) {
            if (rd) {
                for (var i = 0, csnl = csNetwork.length; i < csnl; i++) {
                    if (rd.indexOf(csNetwork[i].toLowerCase()) >= 0)
                        return true;
                }
            }
        }
        return false;
    };

    that.isDirect = function () {
        if (that.ref == '')
            return true;
        if (that.referringDomain && that.rhost.indexOf(that.referringDomain) > -1)
            return true;

        return false;
    };

    that.isBrand = function () {
        if (isBrandRelatedCallBack)
            return isBrandRelatedCallBack(that.ref);
        return false;
    };

    that.isMarketingComs = function () {
        if (that.seg4.indexOf('NEDM') != -1 || that.utm.indexOf('NEDM') != -1)
            return true;
        return false;
    };

    that.isSocialConent = function () {
        if (that.seg4.indexOf('SCDD') != -1 || that.utm.indexOf('SCDD') != -1)
            return true;
        return false;
    };

    that.isProductComs = function () {
        for (var i = 0, pcl = pCom.length; i < pcl; i++) {
            if (that.seg4.indexOf(pCom[i]) >= 0)
                return true;
        }
        return false;
    };

    that.isDisplay = function () {
        if (that.seg4 != '' || that.utm != '')
            return true;
        return false;
    };

    that.isNewSessionWT = function () {
        var c = getCookie("WT_FPC");
        if (c != '') {
            var p = /lv=(.*):/i;
            var n = Number(c.match(p)[1]) + 86400000;
            var t = getTime(0);
            return t > n;
        }
        return true;
    };

    that.isNewSession = function (cattr, lattr) {
        var wts = that.isNewSessionWT();
        if (window._tag && window._tag.CSN && window._tag.CSN.cdx && window._tag.CSN.cdx.context.cdxOccurred) {
            return false;
        }

        if (cattr != 0 && cattr != lattr) {
            return true;
        }
        
        return wts;
    };

    that.getCurrentAttribution = function () {
        if (that.isSearch()) {
            if (that.isPaid()) {
                if (that.isBrand()) {
                    return 1;
                } else {
                    return 2;
                }
            } else {
                if (that.isBrand()) {
                    return 3;
                } else {
                    return 4;
                }
            }
        }
        if (that.isSocialConent()) {
            return 11;
        }
        if (that.isSocial()) {
            return 5;
        }
        if (that.isMarketingComs()) {
            return 8;
        }
        if (that.isProductComs()) {
            return 9;
        }
        if (that.isDisplay()) {
            return 10;
        }
        if (that.isDirect()) {
            return 0;
        }
        if (that.isCsNetwork()) {
            return 6;
        }
        return 7;
    };

    that.referringDomain = getRefDomain();
    that.seg4 = getUrlParam('WT.seg_4');
    that.utm = getUrlParam('utm_source');
    that.cookieTime = getTime(60 * 60 * 24 * 30);

    try {
        loadAttributions();
        var la = getCookie('WT_LA'), fa = getCookie('WT_FA'), ca = that.getCurrentAttribution();
        var ns = that.isNewSession(ca, la);
        ctx.isNewSession = false;
        if (ns) {
            la = ca;

            if (fa == '')
                fa = ca;

            if (attributions[la]) {
                attributions[la]++;
            } else {
                attributions[la] = 1;
            }
            ctx.isNewSession = true;
        }
        setCookie('WT_LA', la, that.cookieTime);
        setCookie('WT_FA', fa, that.cookieTime);
        saveAttributions();

        ctx.lastSeen = tsMap[la];
        ctx.firstSeen = tsMap[fa];
        ctx.mostSeen = tsMap[getHighestAttribution()];
        ctx.sessionCount = sessionCount;
        ctx.utm = that.utm;
        ctx.seg4 = that.seg4;
    }
    catch (err) { }

    return {
        context: ctx,
        debug: that
    };
}