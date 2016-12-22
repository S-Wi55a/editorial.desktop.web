/**
* Slideshow - responsive image slider with touch support
* @module
* @param {object} config - the initialisation config
*/

import LazyLoad from 'vanilla-lazyload'
import dispatchEvent from './utils/dispatch-event.js';

module.exports = function (config = {}) {

    // Default Values
    let settings = Object.assign({
        scope: '._c-slideshow',
        sliderEl: '._c-slideshow__slider',
        sliderNav: '._c-slideshow__nav',
        sliderPageContainer: '._c-slideshow__pages',
        sliderPageButtons: '._c-slideshow__page-button',
        slides: '._c-slideshow__slide',
        slidesImage: '._c-slideshow__image',
        autoSlideTime: 5000,
        pageBy: 1,
        showPages: true,
        showNav: true,
        lazyLoad: false
    }, config);

    let scope = document.querySelector(settings.scope)
    let sliderEl = scope.querySelector(settings.sliderEl)
    let sliderNav = scope.querySelectorAll(settings.sliderNav)
    let sliderPageContainer = scope.querySelectorAll(settings.sliderPageContainer)
    let sliderPageButtons = scope.querySelectorAll(settings.sliderPageButtons)
    let slides = scope.querySelectorAll(settings.slides)
    let slidesTotal = slides.length - 1
    let autoSlideTimer = null
    let isAutoSlide = settings.autoSlide
    let currentSlide = 0
    let canPlay = false

    const MAXFRAMEWIDTH = 100 // Represented as a percentage
    const FIRSTSLIDE = 0 // Index based

    // Show pagination
    if (!settings.showPages && sliderPageContainer.length ) {
        sliderPageContainer[0].style.display = "none"
    }

    // Show Nav (Prev/Next)
    if (!settings.Nav && sliderPageButtons.length ) {
        sliderPageButtons.forEach((button) => {
            button.style.display = "none"
        })
    }

    //Lazy Load
    if (settings.lazyLoad) {

        window.onload = function() {
            let theshold = window.innerWidth

            let lazyLaod = new LazyLoad({
                elements_selector: settings.slidesImage,
                data_src: "src",
                data_srcset: "srcset",
                threshold: theshold

            })
            scope.addEventListener('before.csn-slider.nextSlide', function() {
                lazyLaod.handleScroll();
            });
        }

    }

    /**
     * [dispatchSliderEvent description]
     * @return {[type]} [description]
     */
    function dispatchSliderEvent (phase, type, detail) {
        dispatchEvent(scope, `${phase}.csn-slider.${type}`, detail);
    }


    // Init
    let _init = function() {

        _playVideo() //TODO: check on this

        sliderNav.forEach(item => {
            item.addEventListener('click', (event) => {
                _changeSlide(event.currentTarget.getAttribute('data-direction'))
                isAutoSlide = false;
                _clearAutoSlide();
            })
        })

        if (sliderPageButtons.length) {
            sliderPageButtons.forEach(item => {
                item.addEventListener('click', function (event) {
                    isAutoSlide = false;
                    _clearAutoSlide();
                    let index = parseInt(event.target.getAttribute('data-slide-id'));
                    _switchSlides(index);
                });
            })
        }

        scope.addEventListener('mouseenter', _clearAutoSlide)
        scope.addEventListener('mouseleave', _autoSlide)

        slides.forEach(item => {
            item.style.width = (MAXFRAMEWIDTH/settings.pageBy) + "%" // Must be first or height will be incorrect
            //item.style.height = item.offsetHeight + "px"
        })

        window.addEventListener('resize', () => {
            slides.forEach(item => {
                item.style.height = "auto"
            })
        })

        _autoSlide()
        _switchSlides(currentSlide)
    }

    let _videoEnded = function () {
        // What you want to do after the event
    }

    let _changeSlide = function (direction) {

        let index = currentSlide;

        //Prev
        if (direction === 'prev') {
            if (currentSlide > 0) {
                index = currentSlide - settings.pageBy;
            } else {
                currentSlide = slidesTotal;
                index = currentSlide;
            }
            dispatchSliderEvent('before', 'previousSlide')
        } else {
        // Next
            if ((currentSlide + settings.pageBy) < slidesTotal) {
                index = currentSlide + settings.pageBy;
            } else {
                currentSlide = FIRSTSLIDE;
                index = currentSlide;
            }
            dispatchSliderEvent('before', 'nextSlide')
        }

        _switchSlides(index, direction);
    }

    let _switchSlides = function (index, direction) {

        if (slides[currentSlide].querySelectorAll('._c-bg-video video').length > 0) {

            //TODO: look into dispatch event
            slides[currentSlide].querySelectorAll('._c-bg-video video').trigger('pause');
        }

        currentSlide = index;

        sliderEl.style.transform = 'translate3d(-' + 100 * (currentSlide / settings.pageBy) + '%,0%,0)'
        sliderEl.style.webkitTransform = 'translate3d(-' + 100 * (currentSlide / settings.pageBy) + '%,0%,0)'

        _setActivePage(currentSlide)

        if (direction === 'prev') {
            dispatchSliderEvent('after', 'previousSlide')
        } else {
            dispatchSliderEvent('after', 'nextSlide')
        }

        //TODO: this should be in init
        let prev = scope.querySelector('._c-slideshow__nav--prev');
        if (prev != null) {
            // If its the first slide then disabled the previous arrow
            if (index === 0) {
                prev.setAttribute('data-is-disabled', 'true')
            }
                // else if we are moving to any other slide then enable the previous arrow
            else {
                prev.setAttribute('data-is-disabled', 'false')
            }
        }

        _playVideo();
    }

    let _autoSlide = function () {
        if (isAutoSlide === true && autoSlideTimer === null) {
            autoSlideTimer = setInterval(() => { _changeSlide('next') }, settings.autoSlideTime)
        }
    }

    let _clearAutoSlide = function () {
        clearInterval(autoSlideTimer);
        autoSlideTimer === null
    }

    let _setActivePage = function (index) {
        if (sliderPageButtons.length) {
            sliderPageButtons.forEach(item => {
                item.removeAttribute('data-is-active')
            })
            sliderPageButtons[index].setAttribute('data-is-active', 'true')
        }
    }

    let _playVideo = function () {

        var video = slides[currentSlide].querySelectorAll('._c-bg-video video');

        if (video.length > 0) {

            let shouldKeepAutoSliding = isAutoSlide;
            isAutoSlide = false;


            video.bind('ended', function () {
            });

            if (!video.oncanplay) {
                video.trigger('play');
            }

            video.on('canplay', function () {
                if (!canPlay) {
                    slides[currentSlide].querySelectorAll('._c-bg-video').setAttribute('data-is-active', 'true');
                    canPlay = true;
                    _play(shouldKeepAutoSliding);
                }
            });

            if (canPlay || video.get(0).readyState > 3) {
                slides[currentSlide].querySelectorAll('._c-bg-video').setAttribute('data-is-active', 'true');
                _play(shouldKeepAutoSliding);
            }
        }
    }

    let _play = function (shouldKeepAutoSliding) {
        var video = slides[currentSlide].querySelectorAll('._c-bg-video video');
        video.get(0).currentTime = 0;
        //TODO: remove Jquery ref
        video.trigger('play');
        video.off('ended');
        video.on('ended', function () {
            if (shouldKeepAutoSliding) {
                isAutoSlide = true;
                _autoSlide();

                setTimeout(function () {
                    _changeSlide('next');
                }, 500);
            }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        _init()
    });
}