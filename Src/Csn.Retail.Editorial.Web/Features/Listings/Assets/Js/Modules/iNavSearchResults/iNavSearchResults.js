import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import INavSearchResultsContainer from 'Js/Modules/iNavSearchResults/Containers/iNavSearchResultsContainer'

if (!SERVER) {
    require('Js/Modules/iNavSearchResults/Css/iNavSearchResults.scss')  
}

//Check for Store
const store = window.store

const render = (WrappedComponent) => {
    ReactDOM.render(
        <AppContainer iNavSearchResults>
            <Provider store={store}>
                <WrappedComponent />
            </Provider>
        </AppContainer>,
        document.getElementById('iNavSearchResults')
    );
};


//If store exists
if (store) {
    
    //Render Searchbar Component
    render(INavSearchResultsContainer);

    if (module.hot) {
        module.hot.accept('Js/Modules/iNavSearchResults/Containers/iNavSearchResultsContainer', () => render(INavSearchResultsContainer));
    }
}