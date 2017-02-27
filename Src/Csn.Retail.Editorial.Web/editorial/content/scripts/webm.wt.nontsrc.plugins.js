(function () {
    webmTrafficSource = {
        TrafficSourceAttribution: function (isBrandRelated, tag, windowP, documentP) {
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
                    av;

                if (cc != '') {
                    at = cc.split("|");
                    for (var i = 0, atl = at.length; i < atl; i++) {
                        av = at[i].split('.');
                        attributions[av[0]] = Number(av[1]);
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
                return isBrandRelated;
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
                return false;
            };

            that.isNewSession = function (cattr, lattr) {
                var wts = that.isNewSessionWT();
                if (window.webmCrossDomain && webmCrossDomain.cdxObj && webmCrossDomain.cdxObj.context && webmCrossDomain.cdxObj.context.cdxOccurred && !wts) {
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
                if (that.isCsNetwork()) {
                    return 6;
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
                return 7;
            };

            that.referringDomain = getRefDomain();
            that.seg4 = getUrlParam('WT.seg_4');
            that.utm = getUrlParam('utm_source');
            that.cookieTime = getTime(60 * 60 * 24 * 30);

            try {
                loadAttributions();
                var la = getCookie('WT_LA'), fa = getCookie('WT_FA'), ca = that.getCurrentAttribution();
                var ns = that.isNewSession(ca);
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
                ctx.utm = that.utm;
                ctx.seg4 = that.seg4;
            }
            catch (err) { }

            return {
                context: ctx,
                debug: that
            };
        },
        transformWorker: function (dcs, options) {
            var tsa = new webmTrafficSource.TrafficSourceAttribution(webmIsBrandRelated, dcs);
            dcs.WT.z_ltsrc = tsa.context.lastSeen;
            dcs.WT.z_ftsrc = tsa.context.firstSeen;
            dcs.WT.z_mtsrc = tsa.context.mostSeen;

            var tsrc = webmTrafficSource.trackReferrals(webmIsBrandRelated);
            options.argsa = options.argsa.concat(tsrc);
        },
        trackReferrals: function (isBrandRelated) {
            /* ---------------------
            * LEGACY CODE FROM PANALYSIS - WILL BE REMOVED SOON
			* SEO/SEM Tracking Code
			* --------------------- */
            // version 1.02 2012-02-16 - For www.carsales.com.au

            //Detect if SEM or SEO
            var docLoc = document.location;
            var loc = location.href;
            var urlGet = loc.split('?');
            var docRef = document.referrer;
            var domain = docRef.split('/');
            var trackingType = getURLVariable('trackingType');
            var referrerTypes = { none: 0, internal: 1, external: 2 };
            var socialMediaSites = ['facebook', 'twitter', 'youtube', 'bit'];
            var csNetwork = ["bikesales", "boatsales", "caravancampingsales", "quicksales", "redbook", "motoring", "trucksales", "carpoint", "bikepoint", "boatpoint", "farmmachinerysales", "constructionsales", "livemarket", "discountnewcars"];

            if (!trackingType) {
                var trackingType = 'other'; // default tracking type if none is found in the URL
            };

            // debugging
            var testref = getURLVariable('testref');
            if (testref != "") {
                docRef = decodeURIComponent(testref);
                domain = docRef.split('/');
            };
            // end debugging

            function isSEO() {
                var dr = docRef.toLowerCase(),
				    isSeo = false;

                if (trackingType == 'SEO') {
                    isSeo = true;
                }
                else if (docRef != '') {
                    isSeo = (
					   (dr.indexOf('.google.'))
					   || (dr.indexOf('.bing.') > 0 && dr.indexOf('?q=') > 0)
					   || (dr.indexOf('.yahoo.') > 0 && dr.indexOf('p=') > 0)
					   || (dr.indexOf('.baidu.') > 0 && dr.indexOf('?wd=') > 0)
					   || (dr.indexOf('.search.com') > 0 && dr.indexOf('?q=') > 0)
					   || (dr.indexOf('.aol.') > 0 && dr.indexOf('query=') > 0)
					   || (dr.indexOf('.ask.') > 0 && dr.indexOf('?q=') > 0)
					);
                }

                return isSeo;
            };

            function isSEM() {
                return (isSEO() && docLoc.search.toLowerCase().indexOf('gclid') > 0) || trackingType == 'SEM';
            };

            function isSocial() {
                var sm = socialMediaSites;
                var rd = "", rdLower = "";

                if (domain.length >= 3)
                    rd = domain[2];
                else
                    return false;

                rdLower = rd.toLowerCase();
                for (var i = 0, smLength = sm.length; i < smLength; i++) {
                    if (rdLower.indexOf(sm[i].toLowerCase()) >= 0)
                        return true;
                }
                return false;
            };

            function isCarSalesNetwork() {
                var dr = docRef.toLowerCase();

                for (var i = 0, csnl = csNetwork.length; i < csnl; i++) {
                    if (dr.indexOf(csNetwork[i].toLowerCase()) >= 0)
                        return true;
                }
                return false;
            };

            function getReferralType() {
                var dr = docRef.toLowerCase();

                if (!dr)
                    return referrerTypes.none;

                else if (dr.indexOf('carsales.com.au') > 0 || dr.indexOf('carsalesnetwork.com.au') > 0)
                    return referrerTypes.internal;

                else if (dr.indexOf('http') >= 0)
                    return referrerTypes.external;

                else
                    return referrerTypes.none;
            };

            var referralType = getReferralType(),
			    isSeo = isSEO(),
			    isSem = isSEM(),
			    isBrand = isBrandRelated,
			    zch = "";

            try {
                zch = getZchValFromCookie();
                var urlP = getURLVariable('WT.tsrc');
                if (urlP) {
                    zch = urlP;
                }
                if (!zch) {
                    if (isSeo) {
                        if (isSem && isBrand)
                            zch = "Paid search - Brand related";

                        else if (isSem && !isBrand)
                            zch = "Paid search - Non-brand";

                        else if (!isSem && isBrand)
                            zch = "Organic search - Brand related";

                        else
                            zch = "Organic search - Non-brand";
                    }
                    else if (isSocial())
                        zch = "Social Media";

                    else if (isCarSalesNetwork())
                        zch = "Car Sales Network";

                    else if (!isSem && (referralType == referrerTypes.none || referralType == referrerTypes.internal))
                        zch = "Direct to site";

                    else
                        zch = "Other";
                };

                //			    writeWTMeta(zch);
                setWtSessionCookie(zch); // set a session cookie to flag that the session is active

                var params = ["WT.z_channel", encodeURIComponent(zch)];
                return (params);
            }
            catch (err) { }

            function getURLVariable(name) {
                name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
                var regexS = "[\\?&]" + name + "=([^&#]*)";
                var regex = new RegExp(regexS);
                var results = regex.exec(window.location.href);
                if (results == null)
                    return "";
                else
                    return results[1];
            };

            function writeWTMeta(val) {
                if (val) {
                    document.write('<meta name="WT.z_channel" content="' + encodeURIComponent(val) + '" />');
                    document.write('<meta name="WT.tsrc" content="' + encodeURIComponent(val) + '" />');
                }
            };

            function getZchValFromCookie() {
                var _ucookies = document.cookie.split(";"),
			       _cookieVal = "",
			       np = [],
			       i = 0, _ucl = _ucookies.length;

                for (i = 0; i < _ucl; i++) {
                    np = _ucookies[i].split("=");
                    nc = _trim(np[0]);
                    if (nc.toLowerCase() == '_wtsa') {
                        _cookieVal = decodeURIComponent(_trim(np[1]));
                    }
                }

                return _cookieVal;
            };

            function setWtSessionCookie(cVal) {
                var t = new Date(), cVal = cVal || "1";
                t.setTime(t.getTime() + 1800000);
                document.cookie = "_wtsa=" + cVal + ";expires=" + t.toGMTString() + ';path=/';
            };

            function _trim(val) {
                return val.replace(/^\s+|\s+$/g, '');
            };
        },
        doWork: function (dcs, options) {
            dcs.addTransform(webmTrafficSource.transformWorker, 'all');
        }
    };

    webmCrossDomain = {
        cdxObj: undefined,
        CDX: function (tag, windowP, documentP) {
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
                if (results == null) {
                    var alternateParam = getAlternateParam(name);

                    if (alternateParam != null) {
                        return decodeURIComponent(getUrlParam(alternateParam));
                    }

                    return "";
                }
                else
                    return results[1];
            };

            /*We try to fetch query string value with this alternate parameter, 
            Some cosumers send query string key for crossdomain data transfer in a different format 
            eg "WT_CDX" in case of */
            var getAlternateParam = function (name) {

                var keys = name.split(".");
                if (keys.length != 2) {
                    return null;
                }

                return keys[1] + "_" + keys[0];
            }

            var trim = function (val) {
                return val.replace(/^\s+|\s+$/g, '');
            };

            var setCookie = function (cookieName, value, time) {
                var expires = '', domain = '';
                if (time) {
                    expires = ";expires=" + time.toGMTString();
                }
                if (tag) {
                    domain = ';domain=' + tag.fpcdom;
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
		            search.indexOf('CDX.FA') > -1)
		        {
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
        },

        initializeCdx: function (dcs) {
            if (!webmCrossDomain.cdxObj) {
                webmCrossDomain.cdxObj = new webmCrossDomain.CDX(dcs);
            }
        },

        transformWorker: function (dcs, options) {
            webmCrossDomain.initializeCdx(dcs);
            if (webmCrossDomain.cdxObj && webmCrossDomain.cdxObj.context && webmCrossDomain.cdxObj.context.mId) {
                dcs.WT.dcsaut = dcs.WT.dcsvid = webmCrossDomain.cdxObj.context.mId;
            }
        },
        doWork: function (dcs, options) {
            dcs.addTransform(webmCrossDomain.transformWorker, 'all');
        }
    };

    webmClickTrack = {
        doWork: function (dcs, options) {
            (function ($) {
                "use strict";
                var namespace = "webmclicktracking",
                    events = {
                        click: "click." + namespace
                    },
                    defaultOptions = {
                        pageTag: 'WT.z_pg',
                        sectionTag: 'WT.nv',
                        clickTag: 'WT.z_ti',
                        adclickTag: 'WT.ac'
                    },
                    trackClick = function (section, click, page, adClick) {
                        if (section && click && (typeof (dcsMultiTrack) == 'function')) {
                            var secVal = section.value || 'Page Level';
                            var pageVal = page.value || document.location.pathname;

                            if (!adClick) {
                                dcsMultiTrack('WT.dl', '99', section.tag, secVal, click.tag, click.value, page.tag, pageVal);
                            } else {
                                dcsMultiTrack('WT.dl', '99', section.tag, secVal, click.tag, click.value, page.tag, pageVal, adClick.tag, adClick.value);
                            }
                        }
                    };

                /***** Plugin Methods *****/
                var methods = {
                    init: function (options) {
                        this.each(function () {
                            var $this = $(this);
                            var opts = $.extend({}, defaultOptions, options);
                            $this.data(namespace, opts);
                        });

                        return this;
                    },

                    track: function (sectionValue, clickValue, pageValue, advalue) {
                        var element = $(this),
                            opts = $(this).data(namespace) || defaultOptions,
                            section = {
                                tag: opts.sectionTag,
                                value: sectionValue
                            },
                            click = {
                                tag: opts.clickTag,
                                value: clickValue
                            },
                            page = {
                                tag: opts.pageTag,
                                value: pageValue
                            };
                            var adClick;
                            if (advalue) {
                                adClick = {
                                    tag: opts.adclickTag,
                                    value: advalue
                                };
                            }
                            trackClick(section, click, page, adClick);
                    },

                    destroy: function () {
                        this.each(function () {
                            $(this).off("." + namespace);
                            $(this).data(namespace, null);
                        });
                    }
                };

                /***** Plugin Entry *****/
                $.fn.webmClickTracking = function (method) {

                    if (methods[method]) {
                        return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
                    } else if (typeof method === 'object' || !method) {
                        return methods.init.apply(this, arguments);
                    } else {
                        return null;
                    }
                };

                //Auto wireup
                $(function () {
                    $(document).on(events.click, '[data-webm-clickvalue]', function (e) {
                        var element = $(this),
                            value = element.data('webm-clickvalue'),
                            advalue = element.data('webm-adclickvalue'),
                            section = element.parents('[data-webm-section]:first').data('webm-section'),
                            page = element.parents('[data-webm-page]:first').data('webm-page');
                        element.webmClickTracking("track", section, value, page, advalue);
                    });
                });
            })(jQuery);
            //Plugin end
        }
    };

    stopBounce = {
        transformWorker: function (dcs, options) {
            if (options.args && options.args["WT.z_ep_vt"]) {
                return;
            }
            clearTimeout(window.ep_vtMedium);
            clearTimeout(window.ep_vtSoft);
            dcs.WT.z_ep_vt = 'click';
        },
        doWork: function (dcs, options) {
            dcs.addTransform(stopBounce.transformWorker, 'all');
        }
    };
     
    allPlugins = {
        doWork: function (dcs, options) {
            try {
                webmCrossDomain.initializeCdx(dcs);
            } catch (e) {}
            try {
                dcs.addTransform(webmCrossDomain.transformWorker(dcs, options), 'all');
            } catch (e) {}
            try {
                dcs.addTransform(webmTrafficSource.transformWorker, 'all');
            } catch (e) {}
            try {
                dcs.addTransform(stopBounce.transformWorker, 'all');
            } catch (e) {}
            try {
                webmClickTrack.doWork(dcs, options);
            } catch (e) {}
            try {
                if (typeof (webmPluginsLoaded) == 'function') {
                    webmPluginsLoaded();
                }
            } catch (e) {}
        }
    };
})();

Webtrends.registerPlugin("webm", allPlugins.doWork);