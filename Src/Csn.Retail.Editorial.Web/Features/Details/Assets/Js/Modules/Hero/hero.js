export default function () {

    const multipleImageLayout = document.querySelector('.hero--multipleImages') ? 'MULTIPLE_IMAGE_LAYOUT' : null
    const imageAndVideoLayout = document.querySelector('.hero--imageAndVideo') ? 'IMAGE_VIDEO_LAYOUT' : null
    const singleImageLayout = document.querySelector('.hero--singleImage') ? 'SINGLE_IMAGE_LAYOUT' : null
    const doubleImageLayout = document.querySelector('.hero--doubleImage') ? 'DOUBLE_IMAGE_LAYOUT' : null
    const modalContainer = document.querySelector('._c-modal');

    // Check layout type
    if (multipleImageLayout || imageAndVideoLayout || singleImageLayout || doubleImageLayout) {
        require.ensure(['swiper', 'Js/Modules/Modal/modal.js', 'Js/Modules/Hero/hero-view--modal.js'],
            (require) => {

                const layoutType = multipleImageLayout || imageAndVideoLayout || singleImageLayout || doubleImageLayout

                // Set up sldier and init slider
                initSliderByLayoutType(layoutType, require('swiper'))

                if (modalContainer) {

                    const Modal = require('Js/Modules/Modal/modal.js').default;
                    const View = require('Js/Modules/Hero/hero-view--modal.js');
                    const scope = document.querySelector('.hero');

                    window.csn_modal = window.csn_modal || new Modal()

                    //Add event listeners to hero content
                    const modalTrigger = scope.querySelectorAll('[data-modal-trigger]');
                    for (let item of modalTrigger) {
                        item.addEventListener('click', modalTriggerHandler.bind(null, View, item));
                    }

                }
            },
            'Hero Slidier'
        );
    }
}


function initSliderByLayoutType(layoutType, Swiper) {

    if (layoutType === 'MULTIPLE_IMAGE_LAYOUT') {
        const heroSwiper = new Swiper('.hero .slideshow__container', {
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
            lazyPreloaderClass: 'slideshow__image--preloader',

            //Breakpoints
            breakpoints: {
                1199: {
                    slidesPerView: 2,
                    //Slides grid
                    centeredSlides: false
                }
            }
        })
    } else if (layoutType === 'IMAGE_VIDEO_LAYOUT') {
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
                1199: {
                    slidesPerView: 1,
                }
            }
        })
    } else if (layoutType === 'DOUBLE_IMAGE_LAYOUT') {
        const heroSwiper = new Swiper('.hero .slideshow__container', {
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

        })
    }
}

// Event Handler 
function modalTriggerHandler(View, item, e) {

    e.preventDefault()

    const template = View.template(csn_editorial.hero, item.getAttribute('data-modal-image-index'))

    //update content
    window.csn_modal.show( template, '_c-modal--slideshow', modalGallery );
}

// Modal Gallery
function modalGallery() {

    // This is for GA Gallery tracking requested by the BI team
    // CsnInsightsEventTracker is a global var so we check if it is present first
    if (CsnInsightsEventTracker) {
        const eventContextMetaDataCopy = Object.assign({}, eventContext.metaData);
        eventContextMetaDataCopy.ContentGroup2 = 'gallery';
        CsnInsightsEventTracker.sendPageView(eventContextMetaDataCopy);
    }

    const initialSlide = !!document.querySelector('.slideshow--modal') ? parseInt(document.querySelector('.slideshow--modal').getAttribute('data-slideshow-start')) : 0;

    const swiperOptions = {

        initialSlide: initialSlide,
        roundLengths: true,
        speed: 300,

        // Optional parameters
        loop: true,
        slidesPerView: 1,
        slidesPerGroup: 1,

        //Progress
        watchSlidesProgress: true,
        watchSlidesVisibility: true,

        //Images
        preloadImages: false,
        updateOnImagesReady: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        lazyLoadingInPrevNextAmount: 1, // Can't be less than slidesPerView
        lazyLoadingOnTransitionStart: true,

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

        //Pagination
        pagination: '.slideshow__pagination',
        paginationType: 'fraction',
        paginationHide: false,

        //Callbacks
        onInit: initSliderSize

    }

    // Init slider from swiper
    const modalSwiper = new Swiper('._c-modal .slideshow', swiperOptions)

    // Init styles
    modalSwiper.wrapper[0].style.width = 0;

    //Add event listeners
    modalSwiper.on('onTransitionStart', (swiper) => {
        swiper.wrapper[0].classList.add('swiper-transition')
        modaleResizeHandler(swiper) // Set size of wrapper
    })

    modalSwiper.on('onTransitionEnd', (swiper) => {
        swiper.onResize() // Reset state of swiper 
        swiper.wrapper[0].classList.remove('swiper-transition')
    })

    window.addEventListener('resize', modaleResizeHandler.bind(null, modalSwiper, null))

    window.addEventListener('modal.close', function () {
        window.removeEventListener('resize', modaleResizeHandler)
    })
}

function initSliderSize(swiper) {

    // Find the first image to be displayed
    const _img = swiper.slides[swiper.activeIndex].querySelector('.slideshow__image')

    const src = _img.getAttribute('data-src'),
        srcset = _img.getAttribute('data-srcset'),
        sizes = _img.getAttribute('data-sizes')

    // Once image is loaded, use callback
    swiper.loadImage(_img, src, srcset, sizes, false, function loadImageHandler() {

        // Clean up attributes
        if (srcset) {
            _img.setAttribute('srcset', srcset);
            _img.removeAttribute('data-srcset');
        }
        if (sizes) {
            _img.setAttribute('sizes', sizes);
            _img.removeAttribute('data-sizes');
        }
        if (src) {
            _img.setAttribute('src', src);
            _img.removeAttribute('data-src');
        }

        // Set dimenions for wrapper 
        modaleResizeHandler(swiper, _img)
        // Force transitions
        swiper.slideTo(swiper.activeIndex)


    })
}


// Handler to set the dimensions of the Swiper wrapper
function modaleResizeHandler(swiper, img) {

    img = img || swiper.slides[swiper.activeIndex].querySelector('.slideshow__image')

    let windowHeight = window.innerHeight - 80; //TODO - change to dynamic or var
    let windowWidth = window.innerWidth - 160; //TODO - change to dynamic or var

    let windowRatio = windowWidth / windowHeight;

    let imageRatio = img.naturalWidth / img.naturalHeight;

    swiper.wrapper[0].style.transition = "all 0.300s";

    //console.log(img, img.naturalWidth, img.naturalHeight, imageRatio, windowRatio)
    //imageRatio > 1 = wider
    //imageRatio < 1 = taller

    // If the image is portrait and greater than window size (plus threshold)
    if (windowRatio < imageRatio) {

        let width = windowWidth;
        let height = Math.round((width / imageRatio));

        if (img.naturalWidth > width) {
            swiper.wrapper[0].style.width = (Math.round(width)) + 'px';
            swiper.wrapper[0].style.height = (Math.round(height)) + 'px';


        } else {
            swiper.wrapper[0].style.width = img.naturalWidth + 'px';
            swiper.wrapper[0].style.height = img.naturalHeight + 'px';
        }

    }
    // If the image is landscape and greater than window size (plus threshold)
    else if (windowRatio > imageRatio) {

        let width = Math.round((imageRatio * windowHeight));

        let height = Math.round((width / imageRatio));

        if (img.naturalHeight > height) {
            swiper.wrapper[0].style.width = (Math.round(width)) + 'px';
            swiper.wrapper[0].style.height = (Math.round(height)) + 'px';
        } else {
            swiper.wrapper[0].style.width = img.naturalWidth + 'px';
            swiper.wrapper[0].style.height = img.naturalHeight + 'px';
        }
    }

}