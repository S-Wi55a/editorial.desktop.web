export default function() {
    const multipleImageLayout = document.querySelector('.hero--multipleImages')
    const imageAndVideoLayout = document.querySelector('.hero--imageAndVideo')

    if (multipleImageLayout || imageAndVideoLayout) {
        require.ensure(['swiper', 'Js/Modules/Modal/modal.js'],
    function(require) {
        Swiper = require('swiper')

        if (multipleImageLayout) {
            const heroSwiper = new Swiper('.hero .slideshow__container', {
                // Optional parameters
                loop: true,
                slidesPerView: 3,
                slidesPerGroup: 1,

                //Progress
                watchSlidesProgress: true,
                watchSlidesVisibility: true,

                //Slides grid
                centeredSlides: true,

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
                lazyPreloaderClass: 'slideshow__image--preloader',

                //Breakpoints
                breakpoints: {
                    1200: {
                        slidesPerView: 2,
                        //Slides grid
                        centeredSlides: false
                    }
                }
            })
        } else if (imageAndVideoLayout) {
            const heroSwiper = new Swiper('.hero .slideshow__container', {
                // Optional parameters
                loop: true,
                slidesPerView: 2,
                slidesPerGroup: 1,

                //Progress
                watchSlidesProgress: true,
                watchSlidesVisibility: true,

                //Images
                preloadImages: false,
                lazyLoading: true,
                lazyLoadingInPrevNext: true,
                lazyLoadingInPrevNextAmount: 2, // Can't be less than slidesPerView

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
                lazyPreloaderClass: 'slideshow__image--preloader',

                //Breakpoints
                breakpoints: {
                    1200: {
                        slidesPerView: 1,
                    }
                }
            })
        }


        if (document.querySelector('[data-ajax-modal]')) {

            //init Modal JS
            require('Js/Modules/Modal/modal.js');

            // load the modal slider after the ajax-event has completed //TODO: not using ajax anymore find better name
            window.addEventListener('ajax-completed',
                function() {

                    const initSlide = !!document.querySelector('.slideshow--modal') ? parseInt(document.querySelector('.slideshow--modal').getAttribute('data-slideshow-start')) : 0
                    console.log(initSlide)
                    const modalSwiper = new Swiper('._c-modal .slideshow', {

                        initialSlide: initSlide,

                        // Optional parameters
                        loop: true,
                        slidesPerView: 1,
                        slidesPerGroup: 1,

                        //Progress
                        watchSlidesProgress: true,
                        watchSlidesVisibility: true,

                        //Images
                        preloadImages: false,
                        lazyLoading: true,
                        lazyLoadingInPrevNext: true,
                        lazyLoadingInPrevNextAmount: 1, // Can't be less than slidesPerView

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

                    // Resize Handler for modal window to shrink when window height is less than image
                    let resizeHandlers = function(windowHeightThreshold) {

                        let modalContent = document.querySelector('._c-modal__content');
                        let el = modalContent.querySelector('._c-modal .slideshow');
                        let windowHeight = window.innerHeight - windowHeightThreshold;

                        el.style.width = '';
                        modalSwiper.onResize()

                        // Compare image dimensions with window
                        if (windowHeight < el.offsetHeight) {

                            let imageRatio = 3 / 2;
                            let width = Math.round((imageRatio * windowHeight));

                            el.style.width = (Math.round(width)) + 'px';

                            //then force slider re position
                            modalSwiper.onResize()
                        }
                    }

                    let modalResizeHandler = resizeHandlers.bind(null, 80);

                    // Call it once to get approiate size on first view
                    modalResizeHandler()

                    // handle event
                    window.addEventListener('resize', modalResizeHandler)

                    window.addEventListener('modal.close', function() {
                        window.removeEventListener('resize', modalResizeHandler)

                    })

                });
        }
    },
    'Hero Slidier');
    }
}