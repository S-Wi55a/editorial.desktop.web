import {configureStore, injectAsyncReducer} from 'Redux/Global/Store/store.client.js'

//Check to see if there is a preloaded state
window.__PRELOADED_STATE__ = window.__PRELOADED_STATE__ || {}

//Enable Redux store globally
window.store = configureStore(); //Init store

// load Redux 
(function redux(d) {

    injectAsyncReducer(window.store, 'iNav', require('Redux/iNav/Reducers/iNavReducer').iNavParentReducer);
        
    if (d.querySelector('#iNav')) {
        require('iNav/iNav');
    }
    if (d.querySelector('#iNavBreadcrumbs')) {
        require('iNavBreadCrumbs/iNavBreadCrumbs');
    }
    if (d.querySelector('#iNavSearchResults')) {
        require('iNavSearchResults/iNavSearchResults');
    }

        
    if (module.hot) {
        // Enable Webpack hot module replacement for reducers
        module.hot.accept('Redux/iNav/Reducers/iNavReducer',
            () => {
                injectAsyncReducer(window.store, 'iNav', require('Redux/iNav/Reducers/iNavReducer').iNavParentReducer)
            })
    }
})(document);