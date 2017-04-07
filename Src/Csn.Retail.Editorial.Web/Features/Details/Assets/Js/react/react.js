import React from 'react';
import ReactDOM from 'react-dom';
import Counter from './counter';
import { AppContainer } from 'react-hot-loader';

const app = document.createElement('div');
document.getElementById("react-test-container").appendChild(app);

const render = App => {
    ReactDOM.render(
        <AppContainer><App /></AppContainer>,
        app
    );
};

render(Counter);

if (module.hot) {
    module.hot.accept('./counter', () => render(Counter));
}