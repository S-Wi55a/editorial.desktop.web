// Common css files
require('Css/globalStyles.scss');

import detectIE from 'Js/Modules/DetectIE/detect-ie.js'

// **REQUIRE** - Dynamically set the public path for ajax/code-split requests or webpack won't know where to get chunks
let scripts = document.getElementsByTagName("script");
let scriptsLength = scripts.length;
let patt = /csn\.common/;
for (var i = 0; i < scriptsLength; i++) {
    var str = scripts[i].getAttribute('src');
    if (patt.test(str)) {
        __webpack_public_path__ = str.substring(0, str.lastIndexOf("/")) + '/';
        break;
    }
}

// Get IE or Edge browser version
(function isIE(el, validator){
    let version = validator();
    if (version) {
        window.ie = true
        let ieVersion = version.type + version.number;
        el.classList.toggle(version.type);
        el.classList.toggle(ieVersion);
    }
})(document.body, detectIE);
