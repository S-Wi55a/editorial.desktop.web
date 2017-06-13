// Details Page css files
require('./css/details-page.scss');

//------------------------------------------------------------------------------------------------------------------

//import { loaded } from 'document-promises/document-promises.js';
import ScrollMagic from 'ScrollMagic';

var thenify = function thenify(type, readyState) {
    return new Promise(function (resolve) {
        var listener = function listener() {
            if (readyState.test(document.readyState)) {
                document.removeEventListener(type, listener);

                resolve();
            }
        };

        document.addEventListener(type, listener);

        listener();
    });
};

// export thenfied parsed, contentLoaded, and loaded
var parsed =  thenify('readystatechange', /^(?:interactive|complete)$/);
var contentLoaded =  thenify('DOMContentLoaded', /^(?:interactive|complete)$/);
var loaded = thenify('readystatechange', /^complete$/);



console.log("Loaded: ",loaded)
//------------------------------------------------------------------------------------------------------------------
// Hero
let aboveTheFold = require('Js/Modules/Hero/hero.js').default;
aboveTheFold();

//Editors Rating
(function editorRatings() {
    if (document.querySelector('.editors-ratings')) {
        require('./Js/Modules/EditorsRatings/editorsRating-component.js');
    }
})();

// TEADS
$(function() {
    if ($('#teads-video-container').length) {
        $('#teads-video-container').insertAfter($('.article__copy p:eq(1)'));
    }
});

//Lazy load More articles JS
loaded.then(() => {
    (function moreArticles(d) {

        if (d.querySelector('.more-articles-placeholder')) {
            (function moreArticles() {
                import( /* webpackChunkName: "More-Articles" */ 'Js/Modules/MoreArticles/moreArticles-component.js').then(function(moreArticles) {}).catch(function(err) {
                    console.log('Failed to load More-Articles', err);
                });
            })()
        }
    })(document)
});

//Lazy load Stock For Sale JS
loaded.then(() => {
    (function stockForSale(d) {
        if (d.querySelector('.stock-for-sale-placeholder')) {
            (function stockForSale() {
                import( /* webpackChunkName: "Stock-For-Sale" */ 'Js/Modules/StockForSale/stockForSale-component.js').then(function(stockForSale) {}).catch(function(err) {
                    console.log('Failed to load Stock-For-Sale', err);
                });
            })()
        }
    })(document)
});

//Lazy load Spec Module
loaded.then(() => {
    (function specModule(d) {

        if (csn_editorial.specModule) {
            // Add placeholder 
            let el = d.querySelectorAll('.article__copy p')[1];
            if (el) { el.insertAdjacentHTML('afterend', '<div class="spec-module-placeholder" data-webm-section="spec-module"></div>'); }

            (function specModule() {
                import( /* webpackChunkName: "Spec-Module" */ 'Js/Modules/SpecModule/specModule--container.js').then(function(specModule) {}).catch(function(err) {
                    console.log('Failed to load Spec-Module', err);
                });
            })()
        }
    })(document)
});

//Lazy load Also Consider JS
loaded.then(() => {
    (function alsoConsider(d) {

        if (d.querySelector('.also-consider-placeholder')) {
            (function alsoConsider() {
                import( /* webpackChunkName: "Also-Consider" */ 'Js/Modules/alsoConsider/alsoConsider-component.js').then(function(alsoConsider) {}).catch(function(err) {
                    console.log('Failed to load alsoConsider', err);
                });
            })()
        }

    })(document)
});

//Lazy load Disqus
let disqus = function(d, w, selector) {
    const disqusSelector = d.querySelector(selector);
    if (disqusSelector) {

        // Set scene
        const triggerElement = selector
        const triggerHook = 1
        const offset = (-1 * w.innerHeight) * 2;
        w.scrollMogicController = w.scrollMogicController || new ScrollMagic.Controller();

        new ScrollMagic.Scene({
                triggerElement: triggerElement,
                triggerHook: triggerHook,
                offset: offset,
                reverse: false
            })
            .on("enter", () => { require('Js/Modules/Disqus/disqus.js').default() })
            .addTo(w.scrollMogicController);
    }
}
disqus(document, window, '#disqus_thread');


// load Redux 
//(function redux(d) {

//    if (d.querySelector('#redux-placeholder')) { //TODO: change to iNav check
//        (function iNav() {
//            import( /* webpackChunkName: "iNav-Reducer" */ 'Js/Modules/Redux/iNav/Reducers/iNavParentReducer.js').then(
//                function(iNav) {
//                    window.injectAsyncReducer(window.store, 'iNav', iNav.iNavParentReducer)
//                }).catch(function(err) {
//                console.log('Failed to load iNav', err);
//            });
//        })();
//        (function searchBar() {
//            import( /* webpackChunkName: "SearchBar" */ 'Js/Modules/Redux/iNav/index.js').then(function(searchBar) {}).catch(function(err) {
//                console.log('Failed to load SearchBar', err);
//            });
//        })();
//    }

//    if (process.env.NODE_ENV === 'development') {
//        if (module.hot) {
//            // Enable Webpack hot module replacement for reducers
//            module.hot.accept('Js/Modules/Redux/iNav/Reducers/iNavParentReducer', () => {
//                const nextReducer = require('Js/Modules/Redux/iNav/Reducers/iNavParentReducer').iNavParentReducer
//                window.injectAsyncReducer(window.store, 'iNav', nextReducer)
//            })
//        }
//    }


//})(document)


//Lazy Native Ads
loaded.then(() => {
    (function nativeAds() {
        if (!!csn_editorial && !!csn_editorial.nativeAds) {
            (function nativeAds() {
                import( /* webpackChunkName: "Native Ads" */ 'Js/Modules/NativeAds/nativeAds.js').then(function(nativeAds) {}).catch(function(err) {
                    console.log('Failed to load nativeAds', err);
                });
            })()
        }
    })()
});

//Lazy load Sponsored articles JS
loaded.then(() => {
    (function sponsoredArticles(d) {
        if (d.querySelector('.article__type--sponsored')) {
            (function nativeAds() {
                import( /* webpackChunkName: "Sponsored Articles" */ 'Js/Modules/SponsoredArticles/sponsoredArticles.js').then(function(sponsoredArticles) {}).catch(function(err) {
                    console.log('Failed to load sponsoredArticles', err);
                });
            })()
        }
    })(document)
});