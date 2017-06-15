import Swiper from 'swiper'

// Modal Gallery
export function modalGallery() {

    // This is for GA Gallery tracking requested by the BI team
    // CsnInsightsEventTracker is a global var so we check if it is present first
    if (window.CsnInsightsEventTracker) {
        const eventContextMetaDataCopy = Object.assign({}, window.eventContext.metaData);
        eventContextMetaDataCopy.ContentGroup2 = 'gallery';
        window.CsnInsightsEventTracker.sendPageView(eventContextMetaDataCopy);
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
        onInit: initSliderSize,
        onSlideChangeStart: swiper => swiper.onResize() //This is for IE when modal opens [MOTO-1671]

    }

    // Init slider from swiper
    const modalSwiper = new Swiper('._c-modal .slideshow', swiperOptions)

    // Init styles
    modalSwiper.wrapper[0].style.width = 0;

    //Add event listeners
    modalSwiper.on('onTransitionStart', (swiper) => {
        swiper.wrapper[0].classList.add('swiper-transition')
        modalResizeHandler(swiper) // Set size of wrapper
    })

    modalSwiper.on('onTransitionEnd', (swiper) => {
        swiper.onResize() // Reset state of swiper 
        swiper.wrapper[0].classList.remove('swiper-transition')
    })

    window.addEventListener('resize', modalResizeHandler.bind(null, modalSwiper, null))

    window.addEventListener('modal.close', function () {
        window.removeEventListener('resize', modalResizeHandler)
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
        modalResizeHandler(swiper, _img)
        // Force transitions
        swiper.slideTo(swiper.activeIndex)

    })
}

// Handler to set the dimensions of the Swiper wrapper
function modalResizeHandler(swiper, img) {

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