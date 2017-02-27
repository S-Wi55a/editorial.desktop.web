function WebmBiEventTracker() {
    var extUserCampaignUId = '';
    if ((window.localStorage) && window.localStorage["kxkuid"]) {
        extUserCampaignUId = '&ExtUId=' + window.localStorage["kxkuid"];
    }
    var methods = {
        buildUrl: function (eventid, networkId, verticalType, eventSource, sellerId) {
            if (window.webmInfo && window.webmInfo.biDomain) {

                var eventSourceTag = '';
                if (eventSource) {
                    eventSourceTag = '&EventSource=' + eventSource;
                }
                
                if (!(networkId)) {
                    networkId = 'HS-SELLER-'+ sellerId;
                } else {
                    networkId = 'HS-AD-' + networkId;
                }

                var networkIdTag = '';
                if (networkId) {
                    networkIdTag = '&NetworkId=' + networkId;
                }

                return '//' + window.webmInfo.biDomain + '/fav.ico?SessionId=' + window.webmInfo.biSessionId
                    + '&ServerName=' + window.webmInfo.biServer + '&EventType=' + eventid
                    + '&VerticalType=' + verticalType + eventSourceTag 
                    + networkIdTag + '&UserId=' + window.webmInfo.userId
                    + '&VisitorId=' + window.webmInfo.visitorId + extUserCampaignUId + '&ts=' + new Date().getTime();
            }
        },

        trackClick: function (args) {
            var imgTrack = new Image();
            imgTrack.src = methods.buildUrl(args.eventName, args.networkId, args.VerticalType, args.eventSource, args.sellerId);
        }
    };

    return {
        trackClick: function (args) {
            methods.trackClick(args);
        }
    };
}
