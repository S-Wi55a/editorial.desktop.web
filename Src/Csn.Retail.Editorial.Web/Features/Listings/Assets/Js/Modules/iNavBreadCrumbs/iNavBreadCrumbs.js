import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import INavBreadCrumbsContainer from 'Js/Modules/iNavBreadCrumbs/Containers/iNavBreadCrumbsContainer'

if (!SERVER) {
    require('Js/Modules/iNavBreadCrumbs/Css/iNavBreadCrumbs.scss')  
}

//Check for Store
const store = window.store

const render = (WrappedComponent) => {
    ReactDOM.render(
        <AppContainer iNavBreadcrumbs>
            <Provider store={store}>
                <WrappedComponent />
            </Provider>
        </AppContainer>,
        document.getElementById('iNavBreadcrumbs')
    );
};


//If store exists
if (store) {
    
    //Render Searchbar Component
    render(INavBreadCrumbsContainer);

    if (module.hot) {
        module.hot.accept('Js/Modules/iNavBreadCrumbs/Containers/iNavBreadCrumbsContainer', () => render(INavBreadCrumbsContainer));
    }
}





