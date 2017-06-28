// load Redux 
(function redux(d) {

    if (d.querySelector('#csn-store')) { //TODO: change to iNav check
        window.injectAsyncReducer(window.store, 'iNav', require('Js/Modules/Redux/iNav/Reducers/iNavReducer').iNavParentReducer);
        if (d.querySelector('#iNav')) {
            require('Js/Modules/iNav/iNav.js');
        }
        //if (d.querySelector('#')) {
            require('Js/Modules/iNavBreadCrumbs/iNavBreadCrumbs.js');
        //}

        
        

        if (module.hot) {
            // Enable Webpack hot module replacement for reducers
            module.hot.accept('Js/Modules/Redux/iNav/Reducers/iNavReducer',
                () => {
                    window.injectAsyncReducer(window.store, 'iNav', require('Js/Modules/Redux/iNav/Reducers/iNavReducer').iNavParentReducer)
                })
        }

    }

})(document);