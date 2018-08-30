import { loaded } from 'document-promises/document-promises.js'

loaded.then(() => {
    (function loadGoogleAd($, w) {
        if (w.googletag !== 'undefined' && w.googletag.cmd !== 'undefined') {
            if ($(".googlead-block").length > 0) {
                $(".googlead-block").each(function (index) {
                    var item = $(this);
                    var adSlotId = item.attr('id');
                    var adNetworkId = item.attr('data-ad-network-id');
                    var adUnitId = item.attr('data-ad-unit-id');

                    w.googletag.cmd.push(function() {
                        w.googletag.defineSlot("/" + adNetworkId + "/" + adUnitId, [728, 90], adSlotId)
                            .addService(googletag.pubads());
                    };
                });
                
                w.googletag.cmd.push(function() {
                    w.googletag.pubads().enableSingleRequest();
                    w.googletag.enableServices();
                };
            }

            w.googletag.cmd.push(function() {
                w.googletag.defineSlot("/5276053/SA_Homepage_728x90_M3_Top", [728, 90], "div-gpt-ad-1468849624568-5")
                    .addService(googletag.pubads());
                w.googletag.defineSlot("/5276053/SA_Results_300x250_300x600_R4",
                        [[300, 250], [300, 600]],
                        "div-gpt-ad-1468849624568-8")
                    .addService(googletag.pubads());
                w.googletag.pubads().enableSingleRequest();
                w.googletag.enableServices();

                w.googletag.display('div-gpt-ad-1468849624568-5');
                w.googletag.display('div-gpt-ad-1468849624568-8');
            });
        }
    })(jQuery, window);
});