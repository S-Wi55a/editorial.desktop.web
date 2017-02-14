window.jQuery = require("jquery");
window.$ = window.jQuery;
import'es6-promise/auto';
import detectIE from 'Modules/DetectIE/detect-ie.js'

// Get IE or Edge browser version
let version = detectIE();

if (version) {
    $(document.body).addClass('ie' + ' ' + 'ie' + version);
}
