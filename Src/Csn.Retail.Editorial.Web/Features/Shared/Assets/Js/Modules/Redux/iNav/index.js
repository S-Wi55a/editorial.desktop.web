import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import SearchBar from 'Js/Modules/Redux/iNav/Containers/searchBar'

const store = window.store

const render = SearchBar => {
    ReactDOM.render(
        <AppContainer searchbar>
            <Provider store={store}>
                <SearchBar />
            </Provider>
        </AppContainer>,
        document.getElementById('redux-placeholder')
    );
};

render(SearchBar);

if (module.hot) {
    module.hot.accept('Js/Modules/Redux/iNav/Containers/searchBar', () => render(SearchBar));
}


