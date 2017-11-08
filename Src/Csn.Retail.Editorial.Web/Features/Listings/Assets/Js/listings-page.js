require('Css/listings-page.scss');

import { configureStore } from 'Redux/Global/Store/store.client.js'
import { loaded } from 'document-promises/document-promises.js'
import { reducer as formReducer } from 'redux-form'

//Enable Redux store globally
window.store = configureStore(); //Init store

// load Redux 
(function redux(d) {

    // Check if there is a preloaded state from iNav
    const initState = window.__PRELOADED_STATE__store.listings

    window.store.addReducer('store', require('iNav/Reducers').iNavParentReducer(initState));
    window.store.addReducer('form', formReducer);
        
    if (d.querySelector('#iNav')) {
        require('iNav/iNav');
    }
    if (d.querySelector('#iNavArticleCount')) {
        require('iNavArticleCount/iNavArticleCount');
    }
    if (d.querySelector('#iNavBreadcrumbs')) {
        require('iNavBreadCrumbs/iNavBreadCrumbs');
    }
    if (d.querySelector('#iNavSorting')) {
        require('iNavSorting/iNavSorting');
    }
    if (d.querySelector('#iNavSearchResults')) {
        require('iNavSearchResults/iNavSearchResults');
    }
    if (d.querySelector('#iNavPagination')) {
        require('iNavPagination/iNavPagination');
    }


    if (module.hot) {
        // Enable Webpack hot module replacement for reducers
        module.hot.accept('iNav/Reducers',
            () => {
                window.store.addReducer('store', require('iNav/Reducers').iNavParentReducer)
            })
        module.hot.accept(formReducer,
            () => {
                window.store.addReducer('form', formReducer);
            })
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