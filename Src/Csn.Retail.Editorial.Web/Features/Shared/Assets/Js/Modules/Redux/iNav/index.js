import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import SearchBarContainer from 'Js/Modules/Redux/iNav/Containers/searchBarContainer'

const store = window.store

const render = (WrappedComponent) => {
    ReactDOM.render(
        <AppContainer searchbar>
            <Provider store={store}>
                <WrappedComponent />
            </Provider>
        </AppContainer>,
        document.getElementById('redux-placeholder')
    );
};

render(SearchBarContainer);

if (module.hot) {
    module.hot.accept('Js/Modules/Redux/iNav/Containers/searchBarContainer', () => render(SearchBarContainer));
}


