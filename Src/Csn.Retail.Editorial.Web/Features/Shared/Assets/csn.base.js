// Common css files
require('Css/globalStyles.scss');

import { loaded } from 'document-promises/document-promises'
import * as store  from 'Js/Modules/Redux/Global/Store/store'
import Immutable from 'immutable'


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


// Grab the state from a global variable injected into the server-generated HTML
const preloadedState = window.__PRELOADED_STATE__ 

// Allow the passed state to be garbage-collected
delete window.__PRELOADED_STATE__

//Because ui uses Immutable or we get an error
preloadedState.ui = Immutable.fromJS(preloadedState.ui);

//Enable Redux store globally
window.store = store.configureStore(preloadedState) //Init store
window.injectAsyncReducer = store.injectAsyncReducer
