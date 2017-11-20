import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import INavSortingContainer from 'iNavSorting/Containers/iNavSortingContainer'

//Check for Store
const store = window.store

const render = (WrappedComponent) => {
    ReactDOM.hydrate(
        <AppContainer iNavSorting>
            <Provider store={store}>
                <WrappedComponent />
            </Provider>
        </AppContainer>,
        document.getElementById('iNavSorting')
    );
};


//If store exists
if (store) {
    
    //Render Sorting Component
    render(INavSortingContainer);

    if (module.hot) {
        module.hot.accept('iNavSorting/Containers/iNavSortingContainer', () => render(INavSortingContainer));
    }
}