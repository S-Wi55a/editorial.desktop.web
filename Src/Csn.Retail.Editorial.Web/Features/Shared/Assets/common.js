// Common css files

require('./css/_common.scss');

WebFontConfig = {
    google: {
        families: ['Open Sans:300,400']
    }
};

(function (d) {
    var wf = d.createElement('script'), s = d.scripts[0];
    wf.src = 'https://ajax.googleapis.com/ajax/libs/webfont/1.6.16/webfont.js';
    wf.async = true;
    s.parentNode.insertBefore(wf, s);
})(document);