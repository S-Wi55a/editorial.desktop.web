function WebmAdometryEventTracker(gid, advid) {

    var methods = {
        buildUrl: function (eventid) {
            return '//log.dmtry.com/redir/1/0/' + gid + '/' + eventid + '/0/1/147/0/' + advid + '/1.ver?at=v&d=PxConv';
        },

        trackClick: function (args) {
            var imgTrack = new Image();
            switch (args.eventName) {
                case 'save-search':
                    imgTrack.src = methods.buildUrl('619129');
                    break;
                case 'auto-alert':
                    imgTrack.src = methods.buildUrl('619128');
                    break;
                case 'newsletter-subscribe':
                    imgTrack.src = methods.buildUrl('619130');
                    break;
                case 'loan-app-fixed':
                    imgTrack.src = methods.buildUrl('619132');
                    break;
                case 'loan-app-variable':
                    imgTrack.src = methods.buildUrl('619133');
                    break;
                case 'loan-app-secure':
                    imgTrack.src = methods.buildUrl('619134');
                    break;
                case 'repayment-calc':
                    imgTrack.src = methods.buildUrl('619135');
                    break;
                case 'carfacts-view':
                    imgTrack.src = methods.buildUrl('619136');
                    break;
                default:
            }
        }
    };

    return {
        trackClick: function (args) {
            methods.trackClick(args);
        },
        key: "WebmAdometryEventTracker"
    };
}

function WebmWtEventTracker() {

    var methods = {
        trackEvent: function (arrTags) {
            if (typeof (dcsMultiTrack) === "function") {
                arrTags.push('WT.dl', '99', 'DCS.dcsuri', document.location.pathname.toString().replace('#/', ''));
                dcsMultiTrack.apply(null, arrTags);
            }
        },

        trackVideoClick: function (arrTags) {
            if (typeof (dcsMultiTrack) === "function") {
                arrTags.push('WT.dl', '110', 'DCS.dcsuri', document.location.pathname.toString().replace('#/', ''));
                dcsMultiTrack.apply(null, arrTags);
            }
        },

        trackClick: function (args) {
            var arr = [];

            if (args.adclickTag) {
                arr.push('WT.ac', args.adclickTag);
            }
            switch (args.eventName) {
                case 'number-reveal':
                    arr.push('WT.z_vphone', '1');
                    methods.trackEvent(arr);
                    break;
                case 'carfacts-view':
                    arr.push('WT.z_cf_report', '1');
                    methods.trackEvent(arr);
                    break;
                case 'dealer-show-number':
                    arr.push('WT.z_vphone', '1');
                    methods.trackEvent(arr);
                    break;
                case 'live-connect':
                    arr.push('WT.z_liveconnect', '1');
                    methods.trackEvent(arr);
                    break;
                case 'request-callback':
                    arr.push('WT.z_reqcallback', '1');
                    methods.trackEvent(arr);
                    break;
                case 'dealer-show-generic-details':
                    arr.push('WT.z_vddetails', '1');
                    if (args.make) {
                        arr.push('WT.z_eventmake', args.make);
                    }
                    methods.trackEvent(arr);
                    break;
                case 'dealer-show-address':
                    arr.push('WT.z_vaddress', '1');
                    methods.trackEvent(arr);
                    break;
                case 'dealer-show-map':
                    arr.push('WT.z_vmap', '1');
                    methods.trackEvent(arr);
                    break;
                case 'dealer-oem-logo':
                    arr.push('WT.z_lhub', '1');
                    methods.trackEvent(arr);
                    break;
                case 'gallery-photo-view':
                    arr.push('WT.z_pghit', '1');
                    if ((args.eventInfo) && (args.eventInfo.eventType)) {
                        arr.push('WT.z_evtype');
                        arr.push(args.eventInfo.eventType);
                    }
                    if ((args.eventInfo) && (args.eventInfo.keyCode)) {
                        arr.push('WT.z_keycode');
                        arr.push(args.eventInfo.keyCode);
                    }
                    methods.trackEvent(arr);
                    break;
                case 'gallery-video-view':
                    arr.push('WT.clip_t', 'Stiched Video');

                    if ((args.eventInfo) && (args.eventInfo.videoId)) {
                        arr.push('WT.clip_n');
                        arr.push(args.eventInfo.videoId);
                    }
                    if ((args.eventInfo) && (args.eventInfo.progress !== undefined)) {
                        arr.push('WT.clip_ev');
                        var progressVal = '';
                        if (args.eventInfo.progress.toString() == '0') {
                            progressVal = 'p';
                        }
                        else if (args.eventInfo.progress.toString() == '100') {
                            progressVal = 'f';
                        } else {
                            progressVal = args.eventInfo.progress.toString();
                        }
                        arr.push(progressVal);
                    }
                    methods.trackVideoClick(arr);
                    break;
                default:
            }
        }
    };

    return {
        trackClick: function(args) {
            methods.trackClick(args);
        },
        key : "WebmWtEventTracker"
    }
}

function WebmBiEventTracker() {

    var extUserCampaignUId = '';
    if ((window.localStorage) && window.localStorage["kxkuid"]) {
        extUserCampaignUId = '&ExtUId=' + window.localStorage["kxkuid"];
    }
    var methods = {
        buildUrl: function (eventid, biItem, networkId) {
            if (window.webmInfo && window.webmInfo.biDomain) {
                return '//' + window.webmInfo.biDomain + '/fav.ico?SessionId=' + window.webmInfo.biSessionId + '&ServerName=' + window.webmInfo.biServer + '&EventType=' + eventid + '&Item=' + biItem + '&NetworkId=' + networkId + '&UserId=' + window.webmInfo.userId + '&VisitorId=' + window.webmInfo.visitorId + extUserCampaignUId + '&ts=' + new Date().getTime();
            }

        },

        buildUrlForMediaClick: function (eventid, biItem, networkId, imageUrl, mediaType, seq) {
            if (window.webmInfo && window.webmInfo.biDomain) {
                return '//' + window.webmInfo.biDomain + '/fav.ico?SessionId=' + window.webmInfo.biSessionId
                    + '&ServerName=' + window.webmInfo.biServer + '&EventType=' + eventid
                    + '&Item=' + biItem + '&NetworkId=' + networkId
                    + '&MediaType=' + mediaType + '&Seq=' + seq + '&Name=' + imageUrl
                    + '&UserId=' + window.webmInfo.userId + '&VisitorId=' + window.webmInfo.visitorId + extUserCampaignUId + '&ts=' + new Date().getTime();
            }
        },
        //Params: MediaType= 'V', EventType='V',Name=VideoId,Progress, DealerRefId=<from url>,Item
        buildUrlForVideoClick: function (eventid, biItem, networkId, mediaType, videoId, progress, dealerRefId) {
            if (window.webmInfo && window.webmInfo.biDomain) {
                var dealerInfo = '';

                if (dealerRefId) {
                    dealerInfo = '&DealerRefId=' + dealerRefId;
                }

                return '//' + window.webmInfo.biDomain + '/fav.ico?SessionId=' + window.webmInfo.biSessionId
                    + '&ServerName=' + window.webmInfo.biServer + '&EventType=' + eventid
                    + '&Item=' + biItem + '&NetworkId=' + networkId
                    + '&MediaType=' + mediaType + '&Prog=' + progress + dealerInfo
                    + '&UserId=' + window.webmInfo.userId + '&VisitorId=' + window.webmInfo.visitorId + extUserCampaignUId + '&ts=' + new Date().getTime();
            }
        },

        trackClick: function (args) {
            var imgTrack = new Image();
            switch (args.eventName) {
                case 'carfacts-view':
                    imgTrack.src = methods.buildUrl('CFV', args.biItem, args.eventItem);
                    break;
                case 'dealer-show-number':
                case 'number-reveal':
                    imgTrack.src = methods.buildUrl('PNV', args.biItem, args.eventItem);
                    break;
                case 'live-connect':
                    imgTrack.src = methods.buildUrl('LCT', args.biItem, args.eventItem);
                    break;
                case 'request-callback':
                    imgTrack.src = methods.buildUrl('RCB', args.biItem, args.eventItem);
                    break;
                case 'dealer-show-address':
                    imgTrack.src = methods.buildUrl('ADV', args.biItem, args.eventItem);
                    break;
                case 'dealer-show-generic-details': //View Event for both phone no. and address
                    imgTrack.src = methods.buildUrl('ADGV', args.biItem, args.eventItem);
                    break;
                case 'dealer-show-map':
                    imgTrack.src = methods.buildUrl('MPV', args.biItem, args.eventItem);
                    break;
                case 'gallery-photo-view':
                    var imageUrl;
                    if ((args.eventInfo) && (args.eventInfo.imgUrl)) {
                        imageUrl = args.eventInfo.imgUrl;
                    }
                    var imgSeq;
                    if ((args.eventInfo) && (args.eventInfo.imgSeq)) {
                        imgSeq = args.eventInfo.imgSeq;
                    }
                    imgTrack.src = methods.buildUrlForMediaClick('M', args.biItem, args.eventItem, imageUrl, "P", imgSeq);
                    break;
                case 'gallery-video-view':
                    var videoId;
                    if ((args.eventInfo) && (args.eventInfo.videoId)) {
                        videoId = args.eventInfo.videoId;
                    }
                    var progress;
                    if ((args.eventInfo) && (args.eventInfo.progress !== undefined)) {
                        progress = args.eventInfo.progress;
                    }
                    var dealerRefId;
                    if ((args.eventInfo) && (args.eventInfo.dealerRefId)) {
                        dealerRefId = args.eventInfo.dealerRefId;
                    }
                    imgTrack.src = methods.buildUrlForVideoClick('M', args.biItem, args.eventItem, 'V', videoId, progress, dealerRefId);
                    break;
                default:
            }
        }
    };

    return {
        trackClick: function (args) {
            methods.trackClick(args);
        },
        key: "WebmBiEventTracker"
    };
}
