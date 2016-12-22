// Slideshow

import Slideshow from './src/slideshow.js'


// TODO: turn into if/else or switch
if (document.querySelector('[data-hero-slideshow]') !== null) {
    Slideshow({
        scope: '[data-hero-slideshow]',
        autoSlide: false,
        pageBy: 2,
        showPages: false,
        showNav: false,
        lazyLoad: true
    });
}

if (document.querySelector('[data-details-slideshow]') !== null) {
    Slideshow({
        scope: '[data-details-slideshow]',
        autoSlide: false,
        pageBy: 2,
        showPages: false,
        showNav: false,
        lazyLoad: true
    });
}




if (module.hot) {
    module.hot.accept()
}

