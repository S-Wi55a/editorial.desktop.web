import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import INav from './Containers/iNavContainer'

//Check for Store
const store = window.store

// TODO: extract this out
const render = (WrappedComponent) => {
    ReactDOM.hydrate(
        <AppContainer iNav>
            <Provider store={store}>
                <WrappedComponent />
            </Provider>
        </AppContainer>,
        document.getElementById('iNav')
    );
};


//If store exists
if (store) {
    
    //Render Searchbar Component
    render(INav);

    if (module.hot) {
        module.hot.accept('./Containers/iNavContainer', () => render(INav));
    }
}