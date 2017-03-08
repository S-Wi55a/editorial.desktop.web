// Details Page css files
require('./css/details-page.scss');

//------------------------------------------------------------------------------------------------------------------

import { loaded } from 'document-promises/document-promises.js';
import ScrollMagic from 'ScrollMagic';

//------------------------------------------------------------------------------------------------------------------

// Dynamically set the public path for ajax/code-split requests
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

//------------------------------------------------------------------------------------------------------------------

// Hero
let aboveTheFold = require('Js/Hero/hero.js').default;
aboveTheFold();

//Editors Rating
let editorRatings = function() {
    if (document.querySelector('.editors-ratings')) {
        require('./Js/EditorsRatings/editorsRating-component.js');
    }
}
editorRatings();

// Lazy load Media Motive
let mediaMotive = function () {

    require.ensure(['Js/Modules/MediaMotive/mm.js'],
        function() {
            require('Js/Modules/MediaMotive/mm.js');
        },
        'Media-Motive');
}

/* window is ready */
loaded.then(function () {
    mediaMotive();
});

// TEADS
$(function () {
    if ($('#teads-video-container').length) {
        $('#teads-video-container').insertAfter($('.article__copy p:eq(1)'));

    }
});

//Lazy load More articles JS

let moreArticles = function(d) {

    if (d.querySelector('.more-articles')) {
        require.ensure(['./Js/MoreArticles/moreArticles-component.js'],
        function() {
            require('./Js/MoreArticles/moreArticles-component.js');
        },
        'More-Articles-Component');
    }
}
loaded.then(function () {
    moreArticles(document);
});


//Lazy load More articles JS
let disqus = function(d, w, selector) {
    const disqusSelector = d.querySelector(selector);
    if (disqusSelector) {

        // Set scene
        const triggerElement = selector
        const triggerHook = 1
        const offset = (-1 * w.innerHeight) * 2;
        w.scrollMogicController = w.scrollMogicController || new ScrollMagic.Controller();

        new ScrollMagic.Scene({
            triggerElement: triggerElement,
            triggerHook: triggerHook,
            offset: offset,
            reverse: false
        })
            .on("enter", ()=>{require('Js/Modules/Disqus/disqus.js').default()})
            .addTo(w.scrollMogicController);
    }
}
disqus(document, window, '#disqus_thread');