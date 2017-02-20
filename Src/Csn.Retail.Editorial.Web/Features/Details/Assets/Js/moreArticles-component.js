require('Css/Modules/Lory/_lory.scss');
require('Css/Modules/Widgets/_moreArticles.scss');

import {lory} from 'lory.js';



// Init More Articles Slider
let initMoreArticlesSlider = (selector, options) => {
    const slider = document.querySelector(selector);
    options = Object.assign({
            classNameSlideContainer: 'lory-slider__slides',
            classNameFrame: 'lory-slider__frame'
        },
        options);
    return lory(slider, options);
}

// Toggle hide/show more Articles


// Previous button - with logic if at start

// Next button - w/ logic for lazy loading and if reached end


initMoreArticlesSlider('.more-articles', {
    classNamePrevCtrl: 'more-articles_nav-button--prev',
    classNameNextCtrl: 'more-articles_nav-button--next'
});