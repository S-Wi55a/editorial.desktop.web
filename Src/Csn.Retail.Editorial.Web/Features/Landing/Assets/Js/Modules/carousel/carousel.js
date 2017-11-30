import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import Carousel from 'carousel/Component/carouselComponent'

//Check for Store
const store = window.store

const render = (WrappedComponent) => (container) => {
    ReactDOM.hydrate(
        <AppContainer carousel={container.id} >
            <Provider store={store}>
                <WrappedComponent />
            </Provider>
        </AppContainer>,
        container
    );
};

const component = render(Carousel)

export default component

//If store exists
//if (store) {
    
//    //Render Carousel Components
//    render(Carousel)

//    if (module.hot) {
//        module.hot.accept('iNav/Containers/iNavContainer', () => render(INav));
//    }
//}


