// Common css files
require('Css/globalStyles.scss');

import 'core-js/es6/symbol';
import 'core-js/fn/object/assign';
import 'core-js/fn/dom-collections/iterator';
import 'core-js/fn/promise';
import 'picturefill';

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