import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import SearchBar from 'Js/Modules/iNav/Containers/iNavContainer'
import { watchINavSagaActions } from 'Js/Modules/Redux/iNav/Sagas/iNavSaga'

if (!SERVER) {
    require('Js/Modules/iNav/css/iNav.scss')  
}

//Check for Store
const store = window.store

const render = (WrappedComponent) => {
    ReactDOM.render(
        <AppContainer searchbar>
            <Provider store={store}>
                <WrappedComponent />
            </Provider>
        </AppContainer>,
        document.getElementById('iNav')
    );
};


//If store exists
if (store) {
    //Run sagas for search bar
    window.store.runSaga(watchINavSagaActions)
    
    //Render Searchbar Component
    render(SearchBar);

    if (module.hot) {
        module.hot.accept('Js/Modules/iNav/Containers/iNavContainer', () => render(SearchBar));
    }
}





