import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import INav from 'iNav/Containers/iNavContainer'
import { watchINavSagaActions } from 'Redux/iNav/Sagas/iNavSaga'

if (!SERVER) {
    require('iNav/Css/iNav.scss')  
}

//Check for Store
const store = window.store

// TODO: extract this out
const render = (WrappedComponent) => {
    ReactDOM.render(
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
    //Run sagas for search bar //NOTE: this may be an issues since we will create 3 different sagas
    window.store.runSaga(watchINavSagaActions)
    
    //Render Searchbar Component
    render(INav);

    if (module.hot) {
        module.hot.accept('iNav/Containers/iNavContainer', () => render(INav));
    }
}





