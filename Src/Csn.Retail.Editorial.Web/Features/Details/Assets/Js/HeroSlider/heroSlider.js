import Swiper from 'swiper'

const mySwiper = new Swiper ('.swiper-container', {
    // Optional parameters
    loop: true,
    slidesPerView: 3,
    slidesPerGroup: 1,

    //Progress
    watchSlidesProgress: true,
    watchSlidesVisibility: true,

    //Images
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    lazyLoadingInPrevNextAmount: 3, // Can't be less than slidesPerView

    // Navigation arrows
    nextButton: '.slideshow__nav--next',
    prevButton: '.slideshow__nav--prev',

    //Namespace
    containerModifierClass: 'slideshow',
    wrapperClass: 'slideshow__slides',
    slideClass: 'slideshow__slide',
    lazyLoadingClass: 'slideshow__image',
    lazyStatusLoadingClass: 'slideshow__image--loading',
    lazyStatusLoadedClass: 'slideshow__image--loaded',
    lazyPreloaderClass: 'slideshow__image--preloader'

})

window.mySwiper = mySwiper


// Init More Articles Slider
let initSwiper = (selector, options) => {
    const slider = document.querySelector(selector);
    options = Object.assign({}, options);
    return new Swiper(slider, options);
}