// load Redux 
(function redux(d) {

    if (d.querySelector('#iNav')) { //TODO: change to iNav check
        window.injectAsyncReducer(window.store, 'iNav', require('Js/Modules/Redux/iNav/Reducers/iNavReducer').iNavParentReducer);
        require('Js/Modules/iNav/iNav.js');

        if (module.hot) {
            // Enable Webpack hot module replacement for reducers
            module.hot.accept('Js/Modules/Redux/iNav/Reducers/iNavReducer',
                () => {
                    window.injectAsyncReducer(window.store, 'iNav', require('Js/Modules/Redux/iNav/Reducers/iNavReducer').iNavParentReducer)
                })
        }

    }

})(document);