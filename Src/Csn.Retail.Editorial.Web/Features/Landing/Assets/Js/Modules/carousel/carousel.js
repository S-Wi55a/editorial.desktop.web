import 'react-hot-loader/patch';
import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { AppContainer } from 'react-hot-loader';
import Carousel from 'carousel/Component/carouselComponent'

//Check for Store
const store = window.store

const render = (WrappedComponent) => (container, i) => {
    ReactDOM.hydrate(
        <AppContainer carousel={container.id} >
            <Provider store={store}>
                <WrappedComponent category={container.id} index={i}/>
            </Provider>
        </AppContainer>,
        container
    );
};

export default render(Carousel)


