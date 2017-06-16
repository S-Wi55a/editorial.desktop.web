// Common css files
require('Css/globalStyles.scss');

import "babel-polyfill";
import 'picturefill';

import * as store from 'Js/Modules/Redux/Global/Store/store'
import Immutable from 'immutable'
import detectIE from 'Js/Modules/DetectIE/detect-ie.js'

// Dynamically set the public path for ajax/code-split requests
let scripts = document.getElementsByTagName("script");
let scriptsLength = scripts.length;
let patt = /csn\.base/;
for (var i = 0; i < scriptsLength; i++) {
    var str = scripts[i].getAttribute('src');
    if (patt.test(str)) {
        __webpack_public_path__ = str.substring(0, str.lastIndexOf("/")) + '/';
        break;
    }
}

// Get IE or Edge browser version
let isIE = (el, validator) => {
    let version = validator();
    if (version) {
        window.ie = true
        let ieVersion = 'ie' + version;
        el.classList.toggle('ie');
        el.classList.toggle(ieVersion);
    }
}

isIE(document.body, detectIE);

//Because ui uses Immutable or we get an error
window.__PRELOADED_STATE__.ui = Immutable.fromJS(window.__PRELOADED_STATE__.ui);

//Enable Redux store globally
window.store = store.configureStore(window.__PRELOADED_STATE__) //Init store
window.injectAsyncReducer = store.injectAsyncReducer