require('Css/landing-page.scss');

import { configureStore } from 'Redux/Global/Store/store.client.js'
import { loaded } from 'document-promises/document-promises.js'
import { reducer as formReducer } from 'redux-form'

if (process.env.DEBUG) { require('debug.addIndicators'); }

//Enable Redux store globally
window.store = configureStore(); //Init store

// load Redux 
(function redux(d) {

    // Check if there is a preloaded state
    const initState = window.__PRELOADED_STATE__carousels
    const navInitState = window.__PRELOADED_STATE__store.nav

    window.store.addReducer('carousels', require('carousel/Reducers').carouselParentReducer(initState));
    window.store.addReducer('store', require('ReactComponents/iNav/Reducers').iNavParentReducer(navInitState));
    window.store.addReducer('form', formReducer);

    if (d.querySelector('#iNav')) {
        require('ReactComponents/iNav/iNav');
    }

    if (d.querySelector('.csn-carousel__placeholder')) {

        const carouselsComponent = require('carousel/carousel')
        
        const carousels = [...document.querySelectorAll('.csn-carousel__placeholder')]

        carousels.forEach((carousel, i) => carouselsComponent.carousel(carousel, i))
             
        
    }

})(document);

//Lazy Native Ads
loaded.then(() => {
    (function nativeAds() {
        if (typeof csn_editorial !== 'undefined' && typeof csn_editorial.nativeAds !== 'undefined') {
            (function nativeAds() {
                import
                    (/* webpackChunkName: "Native Ads" */ 'NativeAds/nativeAds.js').then(function(nativeAds) {})
                        .catch(function(err) {
                            console.log('Failed to load nativeAds', err);
                        });
            })();
        }
    })();
});

(function loadGoogleAd() {
    if (googletag !== 'undefined' && googletag.cmd !== 'undefined') {
        googletag.cmd.push(function() {
            googletag.defineSlot("/5276053/SA_Homepage_728x90_M3_Top", [728, 90], "div-gpt-ad-1468849624568-5")
                .addService(googletag.pubads());
            googletag.defineSlot("/5276053/SA_Homepage_300x250_M4", [300, 250], "div-gpt-ad-1468849624568-2")
                .addService(googletag.pubads());
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();

            googletag.display('div-gpt-ad-1468849624568-5');
            googletag.display('div-gpt-ad-1468849624568-2');
        });
    }
})();