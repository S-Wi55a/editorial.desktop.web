import { loaded } from 'document-promises/document-promises.js'
require('css/modules/GoogleAd/_googlead.scss');

loaded.then(() => {
    (function loadGoogleAd($, w) {
        if (w.googletag !== 'undefined' && w.googletag.cmd !== 'undefined') {
            if ($(".googlead-block").length > 0) {
                $(".googlead-block").each(function (index) {
                    var item = $(this);
                    var adSlotId = item.attr('id');
                    var adNetworkCode = item.data('ad-network-code');
                    var adUnitId = item.data('ad-unit-id');
                    var adDimensions = item.data('ad-dimensions').map((dimension) => [dimension.width, dimension.height]);

                    w.googletag.cmd.push(function() {
                        w.googletag.defineSlot("/" + adNetworkCode + "/" + adUnitId, adDimensions, adSlotId)
                            .addService(googletag.pubads());
                    });
                });
                
                w.googletag.cmd.push(function() {
                    w.googletag.pubads().enableSingleRequest();
                    w.googletag.enableServices();
                });

                $(".googlead-block").each(function(index) {
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