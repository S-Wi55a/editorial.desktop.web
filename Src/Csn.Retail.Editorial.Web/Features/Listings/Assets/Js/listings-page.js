import { configureStore } from 'Redux/Global/Store/store.client.js'
import { injectAsyncReducer } from 'Redux/Global/Reducers'


//Enable Redux store globally
window.store = configureStore(); //Init store

// load Redux 
(function redux(d) {

    // Check if there is a preloaded state from iNav
    const initState = window.__PRELOADED_STATE__iNav

    window.store.addReducer('iNav', require('Redux/iNav/Reducers/iNavReducer').iNavParentReducer(initState));

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
        module.hot.accept('Redux/iNav/Reducers/iNavReducer',
            () => {
                injectAsyncReducer(window.store, 'iNav', require('Redux/iNav/Reducers/iNavReducer').iNavParentReducer)
            })
    }
})(document);