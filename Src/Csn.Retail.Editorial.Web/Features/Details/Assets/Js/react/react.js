import React from 'react';
import ReactDOM from 'react-dom';
import SpecModule from './specModule';
import { AppContainer } from 'react-hot-loader';

import 'Css/Modules/Widgets/SpecModule/_specModule.scss'; //TODO: CSS Module


const app = document.createElement('div');
document.querySelector(".spec-module-placeholder").appendChild(app);

const render = App => {
    ReactDOM.render(
        <AppContainer><App /></AppContainer>,
        app
    );
};

render(SpecModule);

if (module.hot) {
    module.hot.accept('./specModule', () => render(SpecModule));
}


//How to remove react hot laod from prod code