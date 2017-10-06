require('Css/listings-page.scss');

import { configureStore } from 'Redux/Global/Store/store.client.js'
import { loaded } from 'document-promises/document-promises.js'

//Enable Redux store globally
window.store = configureStore(); //Init store

// load Redux 
(function redux(d) {

    // Check if there is a preloaded state from iNav
    const initState = window.__PRELOADED_STATE__iNav

    window.store.addReducer('iNav', require('iNav/Reducers').iNavParentReducer(initState));

    if (d.querySelector('#iNav')) {
        require('iNav/iNav');
    }
    if (d.querySelector('#iNavArticleCount')) {
        require('iNavArticleCount/iNavArticleCount');
    }
    if (d.querySelector('#iNavBreadcrumbs')) {
        require('iNavBreadCrumbs/iNavBreadCrumbs');
    }
    if (d.querySelector('#iNavSearchResults')) {
        require('iNavSearchResults/iNavSearchResults');
    }


    if (module.hot) {
        // Enable Webpack hot module replacement for reducers
        module.hot.accept('iNav/Reducers',
            () => {
                window.store.addReducer('iNav', require('iNav/Reducers').iNavParentReducer)
            })
    }
})(document);

//Lazy Native Ads
loaded.then(() => {
    (function nativeAds() {
        if (!!csn_editorial && !!csn_editorial.nativeAds) {
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