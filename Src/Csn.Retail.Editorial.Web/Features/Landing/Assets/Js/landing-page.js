require('Css/landing-page.scss');

import { configureStore } from 'Redux/Global/Store/store.client.js'
import { loaded } from 'document-promises/document-promises.js'

if (process.env.DEBUG) { require('debug.addIndicators'); }

//Enable Redux store globally
window.store = configureStore(); //Init store

// load Redux 
(function redux(d) {

    // Check if there is a preloaded state
    const initState = window.__PRELOADED_STATE__carousels
    const navInitState = window.__PRELOADED_STATE__store.nav

    window.store.addReducer('carousels', require('carousel/Reducers').carouselParentReducer(initState));
    window.store.addReducer('store', require('iNav/Reducers').iNavParentReducer(navInitState));

    if (d.querySelector('#iNav')) {
        require('iNav/iNav');
    }

    if (d.querySelector('.csn-carousel__placeholder')) {
        const carousels = [...document.querySelectorAll('.csn-carousel__placeholder')]
        carousels.forEach((el, i) => require('carousel/carousel').default(el, i))
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