import CustomEvent from 'custom-event'

// get required parameters from query string
var siteName = csn_editorial.nativeAds.siteName
const areaName = csn_editorial.nativeAds.areaName
const makeModel = csn_editorial.nativeAds.makeModel
const setPropertyID = csn_editorial.nativeAds.setPropertyID

// Dynamic import but relies on promise to excetue  
function loadPlacements() {
    import('NativeAds/Area/placements--' + areaName)
        .then(function (detailsPageNativeAds) {
            nativeAds(jQuery, detailsPageNativeAds.placements, detailsPageNativeAds.events)
            const e = new CustomEvent('csn_editorial.nativeAds.ready');
            window.dispatchEvent(e)
        }).catch(function (err) {
            console.log('Failed to load placements', err);
        });
}
loadPlacements();

// Turn into name function to be used a call back
function nativeAds($, placements, registeredEvents) {

    window.NATIVEADS = window.NATIVEADS || {};
    window.NATIVEADS_QUEUE = window.NATIVEADS_QUEUE || [];

    var q = function () {
        return window.NATIVEADS_QUEUE;
    };

    q().push(["setPropertyID", setPropertyID]);

    q().push(["enableMOAT", true, true]);

    // used for tracking internal clicks (sponsored and promoted content hosted internally). The Mediamotive team manually 
    // put the tracknativeads=true parameter onto our nativeAd links so we can track behaviour of users hitting these pages from the native ad links.
    // see https://support.polar.me/hc/en-us/articles/202275040-How-to-enable-MediaVoice-Analytics-Tracking-on-Internal-Content-Pages?flash_digest=bceb3d4343054cc7e0d4793ac3ceb9dd314c1beb
    q().push([
        "configureSecondaryPage",
        {
            track: function () {
                if (location.search.indexOf('tracknativeads=true') >= 0) {
                    return "inbound";
                }
            }
        }
    ]);

    var viewId = getRandomArbitrary();

    // registered events 
    registeredEvents.forEach((event) => {
        window.addEventListener(event, (e) => {

            // loop through each of the placements and check whether we need to push onto this page
            placements.forEach(function (placement) {
                
                // If tile is already applied, skip
                if (placement.active) {
                    return
                }

                // no need to check whether placement exists...polar just ignores
                q().push(["insertPreview", {
                    label: placement.name,
                    unit: {
                        "server": "sas",
                        "host": "mm.carsales.com.au",
                        "customer": "carsales",
                        "site": siteName,
                        "area": areaName,
                        "size": "2x2",
                        "custom": {
                            "tile": placement.placementId,
                            "kuid": KruxSasHelper().getSasData().kuid,
                            "ksg": KruxSasHelper().getSasData().ksg,
                            "random": getRandomArbitrary(),
                            "viewid": viewId,
                            "car": makeModel
                        }
                    },
                    location: placement.location,
                    template: placement.template(e),
                    onRender: function ($element) {
                        console.log('onRender: ', $element)
                    },
                    onFill: function (data) {
                        console.dir('Fill: ', data);
                    },
                    onError: function (error) {
                        console.log('Error: ', error)
                    }
                }]);
            });
        })
    });

    // Need to run once
    registeredEvents.forEach((event) => {
        const e = new CustomEvent(event);
        window.dispatchEvent(e)
    })

    function getRandomArbitrary() {
        return Math.floor(Math.random() * (9999999 - 999999)) + 999999;
    }

};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0], p = d.location.protocol;
    if (d.getElementById(id)) { return; }
    js = d.createElement(s);
    js.id = id; js.type = "text/javascript"; js.async = true;
    js.src = ((p == "https:") ? p : "http:") + "//plugin.mediavoice.com/plugin.js";
    fjs.parentNode.insertBefore(js, fjs);
})(document, "script", "nativeads-plugin");