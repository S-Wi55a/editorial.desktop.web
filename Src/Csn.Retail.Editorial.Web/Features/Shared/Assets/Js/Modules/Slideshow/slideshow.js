// Slideshow

import Slideshow from './slideshow-src.js'

if (document.querySelector('[data-hero-slideshow]') !== null) {
    Slideshow({
        element: '[data-hero-slideshow]',
        autoSlide: false,
        pageBy: 2,
        showPages: false,
        showNav: true
    });
}


if (module.hot) {
    module.hot.accept()
}

