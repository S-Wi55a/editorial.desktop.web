import 'react-hot-loader/patch';
import React from 'react';
import ReactDOM from 'react-dom';
import SpecModule from './specModule';
import { AppContainer } from 'react-hot-loader';
import { proxyEndpoint } from 'Endpoints/endpoints';

import 'Css/Modules/Widgets/SpecModule/_specModule.scss'; //TODO: CSS Module

const specPath = proxyEndpoint();
const GLOBAL_specModuleData = csn_editorial.specVariantsQuery; //Set this to state
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
