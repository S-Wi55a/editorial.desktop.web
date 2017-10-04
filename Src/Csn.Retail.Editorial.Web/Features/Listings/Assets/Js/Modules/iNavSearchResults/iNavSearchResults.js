import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import INavSearchResultsContainer from 'iNavSearchResults/Containers/iNavSearchResultsContainer'

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
        module.hot.accept('iNavSearchResults/Containers/iNavSearchResultsContainer', () => render(INavSearchResultsContainer));
    }
}