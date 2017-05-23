// Details Page css files
require('./css/details-page.scss');

//------------------------------------------------------------------------------------------------------------------

import { loaded } from 'document-promises/document-promises.js';
import ScrollMagic from 'ScrollMagic';

//------------------------------------------------------------------------------------------------------------------

// Hero
let aboveTheFold = require('Js/Modules/Hero/hero.js').default;
aboveTheFold();

//Editors Rating
let editorRatings = function() {
    if (document.querySelector('.editors-ratings')) {
        require('./Js/Modules/EditorsRatings/editorsRating-component.js');
    }
}
editorRatings();

// TEADS
$(function() {
    if ($('#teads-video-container').length) {
        $('#teads-video-container').insertAfter($('.article__copy p:eq(1)'));
    }
});

//Lazy load More articles JS
let moreArticles = function(d) {

    if (d.querySelector('.more-articles-placeholder')) {
        require.ensure(['./Js/Modules/MoreArticles/moreArticles-component.js'],
            function() {
                require('./Js/Modules/MoreArticles/moreArticles-component.js');
            },
            'More-Articles-Component');
    }
}
loaded.then(function() {
    moreArticles(document);
});

//Lazy load Stock For Sale JS
let stockForSale = function(d) {

    if (d.querySelector('.stock-for-sale-placeholder')) {
        require.ensure(['./Js/Modules/StockForSale/stockForSale-component.js'],
            function() {
                require('./Js/Modules/StockForSale/stockForSale-component.js');
            },
            'Stock-For-Sale-Component');
    }
}
loaded.then(function() {
    stockForSale(document);
});

//Lazy load Spec Module
let specModule = function(d) {

    if (csn_editorial.specModule) {
        // Add placeholder 
        let el = d.querySelectorAll('.article__copy p')[1];
        if (el) { el.insertAdjacentHTML('afterend', '<div class="spec-module-placeholder"></div>'); }

        require.ensure(['Js/Modules/SpecModule/specModule--container.js'],
            function() {
                require('Js/Modules/SpecModule/specModule--container.js');
            },
            'Spec-Module-Component');
    }
}
loaded.then(function() {
    specModule(document);
});

//Lazy load Also Consider JS
let alsoConsider = function(d) {

    if (d.querySelector('.also-consider-placeholder')) {
        require.ensure(['./Js/Modules/alsoConsider/alsoConsider-component.js'],
            function() {
                require('./Js/Modules/alsoConsider/alsoConsider-component.js');
            },
            'Also-Consider-Component');
    }
}
loaded.then(function() {
    alsoConsider(document);
});

//Lazy load Disqus
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
            .on("enter", () => { require('Js/Modules/Disqus/disqus.js').default() })
            .addTo(w.scrollMogicController);
    }
}
disqus(document, window, '#disqus_thread');


//Lazy load Redux - //TODO: should me moved to global root reducer that is avaible for other reducers to attach too
let redux = function(d) {

    if (d.querySelector('#redux-placeholder')) {
        require.ensure(['Js/Modules/Redux/Store/store.js'],
            function() {
                require('Js/Modules/Redux/Store/store.js');
            },
            'Redux-Component');
    }
}
loaded.then(function() {
    redux(document);
});
//Lazy Native Ads
let nativeAds = function() {

    if (!!csn_editorial && !!csn_editorial.nativeAds) {
        require.ensure(['Js/Modules/NativeAds/nativeAds.js'],
            function() {
                require('Js/Modules/NativeAds/nativeAds.js');
            },
            'Native Ads');
    }
}
loaded.then(function() {
    nativeAds();
});