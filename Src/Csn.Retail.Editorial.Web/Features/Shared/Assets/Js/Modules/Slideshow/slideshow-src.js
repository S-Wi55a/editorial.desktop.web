/**
* Slideshow - responsive image slider with touch support
* @module
* @param {object} config - the initialisation config
*/

module.exports = function (config) {

    //TODO: merge option with obj assign

    let scope = document.querySelector(config.element)
    let sliderEl = scope.querySelector('._c-slideshow__slider')
    let sliderNav = scope.querySelectorAll('._c-slideshow__nav')
    let sliderPageContainer = scope.querySelectorAll('._c-slideshow__pages')
    let sliderPageButtons = scope.querySelectorAll('._c-slideshow__page-button')
    let slides = scope.querySelectorAll('._c-slideshow__slide')
    let slidesTotal = slides.length
    let currentSlide = 0
    let autoSlideTimer = null
    let doAutoSlide = config.autoSlide
    let pageBy = null
    let showPages = true
    let showNav = true
    const MAXWIDTH = 100

    if (scope.getAttribute('data-current-slide')) {
        currentSlide = parseInt(scope.getAttribute('data-current-slide'))
    }

    if (config.showPages !== null && sliderPageContainer.length ) {
        showPages = config.showPages
        showPages ? "" : sliderPageContainer[0].style.display = "none"
    }

    if (config.showNav !== null) {
        showNav = config.showNav
    }

    let canPlay = false;

    let init = function () {

        slides = scope.querySelectorAll('._c-slideshow__slide')
        slidesTotal = slides.length

        if (config.pageBy) {
            pageBy = config.pageBy
        } else {
            pageBy = 1
        }

        playVideo()

        sliderNav.forEach(item => {
            item.addEventListener('click', (event) => {
                changeSlide(event.currentTarget.getAttribute('data-direction'))
                doAutoSlide = false;
                clearAutoSlide();
            })
        })

        if (sliderPageButtons != null) {
            sliderPageButtons.forEach(item => {
                item.addEventListener('click', function (event) {
                    doAutoSlide = false;
                    clearAutoSlide();
                    let index = parseInt(event.target.getAttribute('data-slide-id'));
                    switchSlides(index);
                });
            })
        }

        scope.addEventListener('mouseenter', clearAutoSlide)
        scope.addEventListener('mouseleave', autoSlide)

        slides.forEach(item => {
            item.style.width = (MAXWIDTH/pageBy) + "%" // Must be first or height will be incorrect
            //item.style.height = item.offsetHeight + "px"
        })

        window.addEventListener('resize', (e) => {
            slides.forEach(item => {
                item.style.height = "auto"
            })
        })

        autoSlide()
        switchSlides(currentSlide)
    }

    let videoEnded = function () {
        // What you want to do after the event
    }

    let changeSlide = function (direction) {

        let index = currentSlide;
        let imageCount = 0; // load more images

        if (direction === 'prev') {
            if (currentSlide > 0) {
                index = currentSlide - 1;
            } else {
                currentSlide == slidesTotal - 1;
                index = currentSlide;
            }
            var event = new Event('previousSlide');
            elem.dispatchEvent(event);
        } else {
            if (currentSlide < (slidesTotal - 1) / pageBy) {
                index = currentSlide + 1;
            } else {
                currentSlide = 0;
                index = currentSlide;
            }

            slides.forEach(slide => {
                if (imageCount < 3) {
                    if (!slide.querySelector('img').getAttribute('src')) {
                        slide.querySelector('img').setAttribute('src', slide.querySelector('img').getAttribute('data-src'))
                    }
                }
            })
        }

        switchSlides(index);
    }


    let playVideo = function () {

        var video = $(slides[currentSlide]).find('._c-bg-video video');

        if (video.length > 0) {

            let shouldKeepAutoSliding = doAutoSlide;
            doAutoSlide = false;


            video.bind('ended', function () {
            });

            if (!video.oncanplay) {
                video.trigger('play');
            }

            video.on('canplay', function () {
                if (!canPlay) {
                    $(slides[currentSlide]).find('._c-bg-video').attr('data-is-active', 'true');
                    canPlay = true;
                    play(shouldKeepAutoSliding);
                }
            });

            if (canPlay || video.get(0).readyState > 3) {
                $(slides[currentSlide]).find('._c-bg-video').attr('data-is-active', 'true');
                play(shouldKeepAutoSliding);
            }
        }
    }

    let play = function (shouldKeepAutoSliding) {
        var video = $(slides[currentSlide]).find('._c-bg-video video');
        video.get(0).currentTime = 0;
        video.trigger('play');
        video.off('ended');
        video.on('ended', function () {
            if (shouldKeepAutoSliding) {
                doAutoSlide = true;
                autoSlide();

                setTimeout(function () {
                    changeSlide('next');
                }, 500);
            }
        });
    }


    let switchSlides = function (index) {

        if ($(slides[currentSlide]).find('._c-bg-video video').length > 0) {
            $(slides[currentSlide]).find('._c-bg-video video').trigger('pause');
        }

        currentSlide = index;

        sliderEl.style.transform = 'translate3d(-' + 100 * currentSlide + '%,0%,0)'
        sliderEl.style.webkitTransform = 'translate3d(-' + 100 * currentSlide + '%,0%,0)'

        if (sliderPageButtons != null) {
            setActivePage(currentSlide)
        }


        let prev = scope.querySelector('._c-slideshow__nav--prev');
        if (prev != null) {
            // If its the first slide then disabled the previous arrow
            if (index == 0) {
                prev.setAttribute('data-is-disabled', 'true')
            }
                // else if we are moving to any other slide then enable the previous arrow
            else {
                prev.setAttribute('data-is-disabled', 'false')
            }
        }

        playVideo();
        broadcastState();
    }

    let autoSlide = function () {
        if (doAutoSlide == true && autoSlideTimer == null) {
            autoSlideTimer = setInterval(() => { changeSlide('next') }, 5000)
        }
    }

    let clearAutoSlide = function () {
        clearInterval(autoSlideTimer);
        autoSlideTimer = null;
    }

    let setActivePage = function (index) {
        if (sliderPageButtons.length) {
            sliderPageButtons.forEach(item => {
                item.removeAttribute('data-is-active')
            })
            sliderPageButtons[index].setAttribute('data-is-active', 'true')
        }
    }


    let broadcastState = function () {
        let ajaxEvt = document.createEvent('Event')

        ajaxEvt.initEvent('slideshow-changed', true, true)
        ajaxEvt.Response = {
            currentSlide: currentSlide,
            slidesTotal: slidesTotal
        }
        window.dispatchEvent(ajaxEvt);
    }

    $(document).ready(function () {
        init()
    });
}