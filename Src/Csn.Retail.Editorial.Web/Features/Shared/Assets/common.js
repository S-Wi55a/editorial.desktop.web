// Common css files

require('./css/Common.scss');


var WebFont = require('webfontloader');
WebFont.load({
    google: {
        families: ['Open Sans:300,400']
    },
    custom: {
        families: ['csnicons'],
        urls: ['/dist/fonts.css']
    }
});