// load Redux 
(function redux(d) {

    window.injectAsyncReducer(window.store, 'iNav', require('Js/Modules/Redux/iNav/Reducers/iNavReducer').iNavParentReducer);
        
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
                window.injectAsyncReducer(window.store, 'iNav', require('Js/Modules/Redux/iNav/Reducers/iNavReducer').iNavParentReducer)
            })
    }


})(document);