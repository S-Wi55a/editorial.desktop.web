﻿/**
* Slideshow - responsive image slider with touch support
* @module
* @param {object} config - the initialisation config
*/

import LazyLoad from 'vanilla-lazyload'
import dispatchEvent from './utils/dispatch-event.js';

module.exports = function (config = {}) {

    const slice = Array.prototype.slice;

    // Default Values
    let settings = Object.assign({
        scope: '._c-slideshow',
        sliderFrame: '._c-slideshow__slider',
        slidesContainer: '._c-slideshow__slides',
        sliderNav: '._c-slideshow__nav',
        sliderPageContainer: '._c-slideshow__pages',
        sliderPageButtons: '._c-slideshow__page-button',
        slides: '._c-slideshow__slide',
        slidesImage: '._c-slideshow__image',
        autoSlideTime: 5000,
        pageBy: 1,
        showPages: true,
        showNav: true,
        autoSlide: false,
        lazyLoad: false,
        infinity: false
    }, config);

    let scope = document.querySelector(settings.scope)
    let sliderFrame = scope.querySelector(settings.sliderFrame)
    let slidesContainer = scope.querySelector(settings.slidesContainer)
    let sliderNav = scope.querySelectorAll(settings.sliderNav)
    let sliderPageContainer = scope.querySelectorAll(settings.sliderPageContainer)
    let sliderPageButtons = scope.querySelectorAll(settings.sliderPageButtons)
    let slides = scope.querySelectorAll(settings.slides)
    let slidesTotal = slides.length
    let autoSlideTimer = null
    let isAutoSlide = settings.autoSlide
    let currentSlide = 0
    let pageBy = scope.getAttribute('data-slideshow-page-by') || settings.pageBy
    let doOnce = true;
    let toggle = 0
    let reverseToggle = 0
    let isOdd =  false
    let firstSlide = 0 // Index based

    pageBy = parseInt(pageBy)

    const MAXFRAMEWIDTH = 100 // Represented as a percentage

    function once(fn, context) {
        var result;

        return function() {
            if(fn) {
                result = fn.apply(context || this, arguments);
                fn = null;
            }

            return result;
        };
    }

    /**
     * [dispatchSliderEvent description]
     * @return {[type]} [description]
     */
    function dispatchSliderEvent (phase, type, detail) {
        dispatchEvent(scope, `${phase}.csn-slider.${type}`, detail);
    }


    /**
     * private
     * setupInfinite: function to setup if infinite is set
     *
     * @param  {array} slideArray
     */
    function setupInfinite (slideArray, howManyToClone) {

        let front = null;
        let back = slideArray.slice(slideArray.length - howManyToClone, slideArray.length);

        if ((slideArray.length - 1) % 2) {
            front = slideArray.slice(0, howManyToClone);
        } else {
            front = slideArray.slice(0, howManyToClone + 1);
            isOdd = true
        }

        front.forEach(function (element) {
            const cloned = element.cloneNode(true);

            slidesContainer.appendChild(cloned);
        });

        back.reverse()
            .forEach(function (element) {
                const cloned = element.cloneNode(true);

                slidesContainer.insertBefore(cloned, slidesContainer.firstChild);
            });

    }


    // Init
    function _init() {

        // Show pagination
        if (!settings.showPages && sliderPageContainer.length ) {
            sliderPageContainer[0].style.display = "none"
        }

        // Show Nav (Prev/Next)
        if (!settings.showNav && sliderNav.length || slidesTotal <= pageBy ) {
            sliderNav.forEach((button) => {
                button.style.display = "none"
            })
        }

        //Lazy Load
        if (settings.lazyLoad) {

            let theshold = window.innerWidth

            let toBeCalledOnce = once(function() {
                console.log('ran once')
                slidesContainer.style.width = sliderFrame
                    .offsetWidth +
                    "px" // To ensure slides are translating with whole numbers

            });

            let lazyLaod = new LazyLoad({
                elements_selector: settings.slidesImage,
                data_src: "src",
                data_srcset: "srcset",
                threshold: theshold,
                callback_load: toBeCalledOnce
            })

            lazyLaod.handleScroll();

            ['after.csn-slider.previousSlide', 'after.csn-slider.nextSlide'].forEach(function(e) {
                scope.addEventListener(e, function() {
                    lazyLaod.handleScroll();
                });
            });

        }

        //Setup infinity
        if (settings.infinity) {
            setupInfinite(slice.call(slidesContainer.children), pageBy)
            slides = scope.querySelectorAll(settings.slides)
            slidesTotal = slides.length
            firstSlide = pageBy
            currentSlide = firstSlide;
        }

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
            item.style.width = (MAXFRAMEWIDTH/pageBy) + "%"
        })

        // Resize
        window.addEventListener('resize', () => {
            slidesContainer.style.width = "auto" // Hack to get correct image size
            slidesContainer.style.width = sliderFrame.offsetWidth + "px"
            slides.forEach(item => {
                item.style.height = "auto"
            })
        })

        //_autoSlide()

        //NOTE: Does rewind by default
        //let prev = scope.querySelector('._c-slideshow__nav--prev');
        //if (prev != null) {
        //    // If its the first slide then disabled the previous arrow
        //    if (index === 0) {
        //        prev.setAttribute('data-is-disabled', 'true')
        //    }
        //        // else if we are moving to any other slide then enable the previous arrow
        //    else {
        //        prev.setAttribute('data-is-disabled', 'false')
        //    }
        //}

        if (settings.infinity) {
            _animateSliding((currentSlide / pageBy), 0)
        } else {
            _switchSlides(currentSlide)
        }

    }


    let _animateSliding = function (target, duration) {
        // Transition slider to the target page
        duration = (duration != undefined ? duration : timing) + 's'
        slidesContainer.style.transitionDuration = duration
        slidesContainer.style.webkitTransitionDuration = duration
        slidesContainer.style.transform = 'translate3d(-' + 100 * target + '%,0%,0)'
        slidesContainer.style.webkitTransform = 'translate3d(-' + 100 * target + '%,0%,0)'
    }

    let _changeSlide = function (direction) {

        let index = currentSlide;

        //Prev
        if (direction === 'prev') {
            if (currentSlide > 0) {
                index = currentSlide - pageBy;
            } else {
                currentSlide = slidesTotal - 1; //Last slide
                index = currentSlide;
            }
            dispatchSliderEvent('before', 'previousSlide')
        } else {
        // Next
            if ((currentSlide + pageBy) < slidesTotal) {
                index = currentSlide + pageBy;
            } else {
                currentSlide = firstSlide;
                index = currentSlide;
            }
            dispatchSliderEvent('before', 'nextSlide')
        }

        _switchSlides(index, direction);
    }

    let _switchSlides = function (index, direction) {

        currentSlide = index;

        let evtArr = ['webkitTransitionEnd', 'transitionEnd'];

        if (settings.infinity) {
            let clonedSlides = pageBy

            let needsSwapping = currentSlide === reverseToggle || currentSlide === slidesTotal - pageBy - toggle

            if (needsSwapping) {

                // Odd logic only works for pageBy <= 2
                if (isOdd) { toggle = toggle ? 0 : 1 }

                let slideAniHandler = function() {

                    let nextPage = currentSlide === reverseToggle ? slidesTotal - (pageBy+clonedSlides+toggle) : pageBy + toggle

                    if (isOdd) { reverseToggle = reverseToggle ? 0 : 1 }

                    _animateSliding((nextPage / pageBy), 0)
                    currentSlide = nextPage

                    // Remove listener on self
                    evtArr.forEach(function(e) {
                        slidesContainer.removeEventListener(e, slideAniHandler);
                    });
                };

                evtArr.forEach(function(e) {
                    slidesContainer.addEventListener(e, slideAniHandler);
                });
            }
        }

        _animateSliding((currentSlide / pageBy), 0.5)

        _setActivePage(currentSlide)


        // TODO: should be init
        if (doOnce) {
            evtArr.forEach(function(e) {
                slidesContainer.addEventListener(e, function() {
                    if (direction === 'prev') {
                        dispatchSliderEvent('after', 'previousSlide')
                    } else {
                        dispatchSliderEvent('after', 'nextSlide')
                    }
                });
            });
            doOnce = false;
        }
    }

    function _autoSlide() {
        if (isAutoSlide === true && autoSlideTimer === null) {
            autoSlideTimer = setInterval(() => { _changeSlide('next') }, settings.autoSlideTime)
        }
    }

    function _clearAutoSlide() {
        clearInterval(autoSlideTimer);
        autoSlideTimer === null
    }

    function _setActivePage(index) {
        if (sliderPageButtons.length) {
            sliderPageButtons.forEach(item => {
                item.removeAttribute('data-is-active')
            })
            sliderPageButtons[index].setAttribute('data-is-active', 'true')
        }
    }

    if (document.readyState === "interactive" || document.readyState === 'complete') {
        _init()
    } else {
        document.addEventListener('DOMContentLoaded', function () {
            _init()
        });
    }

}