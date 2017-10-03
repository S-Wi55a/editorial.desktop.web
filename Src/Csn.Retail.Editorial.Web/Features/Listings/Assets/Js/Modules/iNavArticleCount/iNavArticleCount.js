import 'react-hot-loader/patch';
import React from 'react'
import { Provider } from 'react-redux'
import ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import INavArticleCountComponent from 'iNavArticleCount/Components/iNavArticleCountComponent'

//Check for Store
const store = window.store

const render = (WrappedComponent) => {
    ReactDOM.render(
        <AppContainer iNavArticleCount>
            <Provider store={store}>
                <WrappedComponent />
            </Provider>
        </AppContainer>,
        document.getElementById('iNavArticleCount')
    );
};


//If store exists
if (store) {
    
    //Render Searchbar Component
    render(INavArticleCountComponent);

    if (module.hot) {
        module.hot.accept('iNavArticleCount/Components/iNavArticleCountComponent', () => render(INavArticleCountComponent));
    }
}

