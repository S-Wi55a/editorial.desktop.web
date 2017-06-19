import 'react-hot-loader/patch';
import React from 'react';
import ReactDOM from 'react-dom';
import SpecModule from './specModule';
import { AppContainer } from 'react-hot-loader';
import { proxy } from 'Js/Modules/Endpoints/endpoints';

import 'Css/Modules/Widgets/SpecModule/_specModule.scss'; //TODO: CSS Module

const specPath = proxy;
const GLOBAL_specModuleData = csn_editorial.specModule; //Set this to state
window.csn_modal = window.csn_modal || new Modal()


const app = document.createElement('div');
document.querySelector(".spec-module-placeholder").appendChild(app);

const render = SpecModule => {
    ReactDOM.render(
        <AppContainer><SpecModule path={specPath} data={GLOBAL_specModuleData} modal={window.csn_modal}/></AppContainer>,
        app
    );
};

render(SpecModule);

if (module.hot) {
    module.hot.accept('./specModule', () => render(SpecModule));
}
