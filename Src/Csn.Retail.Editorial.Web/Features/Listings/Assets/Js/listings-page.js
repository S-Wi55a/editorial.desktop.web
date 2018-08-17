require('Css/listings-page.scss');

import { configureStore } from 'Redux/Global/Store/store.client.js'
import { loaded } from 'document-promises/document-promises.js'
import { reducer as formReducer } from 'redux-form'
import * as isMobile from 'ismobilejs'
if (process.env.DEBUG) { require('debug.addIndicators'); }

//Enable Redux store globally
window.store = configureStore(); //Init store

// load Redux 
(function redux(d) {

    // Check if there is a preloaded state from iNav
    const initState = window.__PRELOADED_STATE__store.nav

    window.store.addReducer('store', require('ReactComponents/iNav/Reducers').iNavParentReducer(initState));
    window.store.addReducer('form', formReducer);
        
    if (d.querySelector('#iNav')) {
        require('ReactComponents/iNav/iNav');
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
        module.hot.accept('ReactComponents/iNav/Reducers',
            () => {
                window.store.addReducer('store', require('ReactComponents/iNav/Reducers').iNavParentReducer)
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

//Sticky Sidebar
if(!document.querySelector('body').classList.contains('ie') && !isMobile.tablet && !isMobile.phone){
    const aside = document.querySelector('.aside')
    loaded.then(function() {     
        if (aside) {
            require('Js/Modules/StickySidebar/stickySidebar.js').init(document, window, aside, '.main', 30, 137 )
        }
    })
}

(function loadGoogleAd() {
    if (googletag !== 'undefined' && googletag.cmd !== 'undefined') {
        googletag.cmd.push(function() {
            googletag.defineSlot("/5276053/SA_Homepage_728x90_M3_Top", [728, 90], "div-gpt-ad-1468849624568-5")
                .addService(googletag.pubads());
            googletag.defineSlot("/5276053/SA_Results_300x250_300x600_R4", [[300, 250], [300, 600]], "div-gpt-ad-1468849624568-8")
                .addService(googletag.pubads());
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();

            googletag.display('div-gpt-ad-1468849624568-5');
            googletag.display('div-gpt-ad-1468849624568-8');
        });
    }
})();