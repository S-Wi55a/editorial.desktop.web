import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import INavPaginationContainer from 'iNavPagination/Containers/iNavPaginationContainer'

//Check for Store
const store = window.store

const render = (WrappedComponent) => {
    ReactDOM.hydrate(
        <AppContainer iNavPagination>
            <Provider store={store}>
                <WrappedComponent />
            </Provider>
        </AppContainer>,
        document.getElementById('iNavPagination')
    );
};


//If store exists
if (store) {
    
    //Render Pagination Component
    render(INavPaginationContainer);

    if (module.hot) {
        module.hot.accept('iNavPagination/Containers/iNavPaginationContainer', () => render(INavPaginationContainer));
    }
}