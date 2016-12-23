// Slideshow

import Slideshow from './src/slideshow.js'

if (document.querySelector('[data-slideshow]')) {
    Slideshow({
        scope: '[data-slideshow]',
        showPages: false,
        lazyLoad: true
    });
}

if (module.hot) {
    module.hot.accept()
}

