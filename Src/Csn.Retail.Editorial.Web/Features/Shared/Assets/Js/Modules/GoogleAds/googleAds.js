import { loaded } from 'document-promises/document-promises.js'
require('css/modules/GoogleAds/_googleads.scss');

loaded.then(() => {
    (function loadGoogleAd($, w) {
        if (w.googletag !== 'undefined' && w.googletag.cmd !== 'undefined') {
            if ($(".googleads-block").length > 0) {
                $(".googleads-block").each(function (index) {
                    var item = $(this);
                    var adSlotId = item.attr('id');
                    var adNetworkCode = item.attr('data-ad-network-code');
                    var adUnitId = item.attr('data-ad-unit-id');
                    var adWidth = item.attr('data-ad-width');
                    var adHeight = item.attr('data-ad-height');

                    w.googletag.cmd.push(function() {
                        w.googletag.defineSlot("/" + adNetworkCode + "/" + adUnitId, [parseInt(adWidth), parseInt(adHeight)], adSlotId)
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