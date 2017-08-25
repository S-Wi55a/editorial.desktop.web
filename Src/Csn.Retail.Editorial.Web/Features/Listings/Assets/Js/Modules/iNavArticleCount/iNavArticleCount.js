import React from 'react'
import { Provider } from 'react-redux'
import ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import INavArticleCountComponent from 'Js/Modules/iNavArticleCount/Components/iNavArticleCountComponent'

if (!SERVER) {
    require('Js/Modules/iNavArticleCount/Css/iNavArticleCount.scss')
}

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
        module.hot.accept('Js/Modules/iNavArticleCount/Components/iNavArticleCountComponent', () => render(INavArticleCountComponent));
    }
}

