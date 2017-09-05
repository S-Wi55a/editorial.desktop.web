import {configureStore, injectAsyncReducer} from 'Js/Modules/Redux/Global/Store/store.client.js'

//Check to see if there is a preloaded state
window.__PRELOADED_STATE__ = window.__PRELOADED_STATE__ || {}

//Enable Redux store globally
window.store = configureStore(); //Init store

// load Redux 
(function redux(d) {

    injectAsyncReducer(window.store, 'iNav', require('Js/Modules/Redux/iNav/Reducers/iNavReducer').iNavParentReducer);
        
    if (d.querySelector('#iNav')) {
        require('Js/Modules/iNav/iNav');
    }
    if (d.querySelector('#iNavBreadcrumbs')) {
        require('Js/Modules/iNavBreadCrumbs/iNavBreadCrumbs');
    }
    if (d.querySelector('#iNavSearchResults')) {
        require('Js/Modules/iNavSearchResults/iNavSearchResults');
    }

        
    if (module.hot) {
        // Enable Webpack hot module replacement for reducers
        module.hot.accept('Js/Modules/Redux/iNav/Reducers/iNavReducer',
            () => {
                injectAsyncReducer(window.store, 'iNav', require('Js/Modules/Redux/iNav/Reducers/iNavReducer').iNavParentReducer)
            })
    }
})(document);