require('Css/listings-page.scss');

import { configureStore } from 'Redux/Global/Store/store.client.js'
import { reducer as formReducer } from 'redux-form'

//Enable Redux store globally
window.store = configureStore(); //Init store

// load Redux 
(function redux(d) {

    // Check if there is a preloaded state from iNav
    const initState = window.__PRELOADED_STATE__iNav

    window.store.addReducer('iNav', require('iNav/Reducers').iNavParentReducer(initState));
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
                window.store.addReducer('iNav', require('iNav/Reducers').iNavParentReducer)
            })
        module.hot.accept(formReducer,
            () => {
                window.store.addReducer('form', formReducer);
            })
    }
})(document);