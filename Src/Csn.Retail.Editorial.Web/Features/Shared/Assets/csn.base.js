// Common css files
require('Css/globalStyles.scss');

import { loaded } from 'document-promises/document-promises.js';

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

//Lazy load Redux 
let redux = function (d) {

    if (d.querySelector('#redux-placeholder')) {
        require.ensure(['Js/Modules/Redux/Global/Store/store.js'],
            function () {
                require('Js/Modules/Redux/Global/Store/store.js');
            },
            'Redux-Component');
    }
}
loaded.then(function () {
    redux(document);
});