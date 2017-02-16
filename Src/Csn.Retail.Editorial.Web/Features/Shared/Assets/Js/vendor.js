window.jQuery = require("jquery");
window.$ = window.jQuery;

import'es6-promise/auto';
import detectIE from 'Js/Modules/DetectIE/detect-ie.js'

// Get IE or Edge browser version
let isIE = (el, validator) => {
    let version = validator();
    if (version) {
        let ieVersion = 'ie' + version;
        el.classList.toggle('ie');
        el.classList.toggle(ieVersion);
    }
}

isIE(document.body, detectIE);