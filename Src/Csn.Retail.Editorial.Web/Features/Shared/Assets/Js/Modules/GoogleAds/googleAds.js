import { loaded } from 'document-promises/document-promises.js'
require('css/modules/GoogleAds/_googleads.scss');

loaded.then(() => {
    (function loadGoogleAd($, w) {
        if (w.googletag !== 'undefined' && w.googletag.cmd !== 'undefined') {
            if ($(".googleads-block").length > 0) {
                $(".googleads-block").each(function (index) {
                    var item = $(this);
                    var adSlotId = item.attr('id');
                    var adNetworkId = item.attr('data-ad-network-id');
                    var adUnitId = item.attr('data-ad-unit-id');

                    w.googletag.cmd.push(function() {
                        w.googletag.defineSlot("/" + adNetworkId + "/" + adUnitId, [728, 90], adSlotId)
                            .addService(googletag.pubads());
                    });
                });
                
                w.googletag.cmd.push(function() {
                    w.googletag.pubads().enableSingleRequest();
                    w.googletag.enableServices();
                });

                $(".googleads-block").each(function(index) {
                    var item = $(this);
                    var adSlotId = item.attr('id');
                    w.googletag.cmd.push(function() {
                        w.googletag.display(adSlotId);
                    });
                });
            }
        }
    })(jQuery, window);
});