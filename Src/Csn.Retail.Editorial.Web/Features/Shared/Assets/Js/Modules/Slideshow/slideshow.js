// Slideshow

import Slideshow from './src/slideshow.js'

if (document.querySelector('[data-slideshow]')) {
    Slideshow({
        scope: '[data-slideshow]',
        autoSlide: false,
        showPages: false,
        showNav: false,
        lazyLoad: true
    });
}

if (module.hot) {
    module.hot.accept()
}

