// Dynamically set the public path for ajax/code-split requests
let resourceURL = '';
let scripts = document.getElementsByTagName("script");
let scriptsLength = scripts.length;
for (var i = 0; i < scriptsLength; i++) {
    var str = scripts[i].getAttribute('src');
    if (/csn\.common/.test(str)) {
        resourceURL = str.substring(0, str.lastIndexOf("/")) + '/';
        break;
    }
}

// Common css files
require('./css/common.scss');

window.jQuery = require("jquery");
window.$ = window.jQuery;

var WebFont = require('webfontloader');



WebFont.load({
    google: {
        families: ['Open Sans:300,400']
    },
    custom: {
        families: ['csnicons'],
        urls: [resourceURL + 'fonts.css']
    }
});