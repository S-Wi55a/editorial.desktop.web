import { modalGallery } from 'Hero/hero-modal.js'
import Swiper from 'swiper'
import Modal from 'Modal/modal.js'
import { template as modalView } from 'Hero/hero-modal--view.js'

export default function () {

    const multipleImageLayout = document.querySelector('.hero--multipleImages') ? 'MULTIPLE_IMAGE_LAYOUT' : null
    const imageAndVideoLayout = document.querySelector('.hero--imageAndVideo') ? 'IMAGE_VIDEO_LAYOUT' : null
    const singleImageLayout = document.querySelector('.hero--singleImage') ? 'SINGLE_IMAGE_LAYOUT' : null
    const doubleImageLayout = document.querySelector('.hero--doubleImage') ? 'DOUBLE_IMAGE_LAYOUT' : null
    const modalContainer = document.querySelector('._c-modal');

    const layoutType = multipleImageLayout || imageAndVideoLayout || singleImageLayout || doubleImageLayout

    // Check if gallery modal is enabled
    const galleryModalEnabled = !csn_editorial.detailsModal; // Disable gallery modal if its details modal page

    // Set up slider and init slider
    initSliderByLayoutType(layoutType);

    if (modalContainer) {
        const scope = document.querySelector('.hero');

        if (!scope) return;

        if (galleryModalEnabled) {
            window.csn_modal = window.csn_modal || new Modal()

            //Add event listeners to hero content
            const modalTrigger = scope.querySelectorAll('[data-modal-trigger]');
            for (let item of modalTrigger) {
                item.addEventListener('click', modalTriggerHandler.bind(null, item));
            }
        } else {
            const images = scope.querySelectorAll('.slideshow__image');
            for (let item of images) {
                item.style.cursor = 'default';
            }
            const viewPhotosButton = document.querySelector('.slideshow__view-photos');
            if(viewPhotosButton) viewPhotosButton.style.display = 'none';
        }
    }
}

function initSliderByLayoutType(layoutType) {

    let swiperOptions = {
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

    }

    if (layoutType === 'MULTIPLE_IMAGE_LAYOUT') {

        swiperOptions = Object.assign({}, swiperOptions, {
            slidesPerView: 3,
            //Breakpoints
            breakpoints: {
                1199: {
                    slidesPerView: 2,
                    //Slides grid
                    centeredSlides: false
                }
            }
        })

        const heroSwiper = new Swiper('.hero .slideshow__container', swiperOptions)

    } else if (layoutType === 'IMAGE_VIDEO_LAYOUT') {

        swiperOptions = Object.assign({}, swiperOptions, {
            slidesPerView: 2,
            //Breakpoints
            breakpoints: {
                1199: {
                    slidesPerView: 1,
                }
            }
        })
        const heroSwiper = new Swiper('.hero .slideshow__container', swiperOptions)
    } else if (layoutType === 'DOUBLE_IMAGE_LAYOUT') {
        const heroSwiper = new Swiper('.hero .slideshow__container', swiperOptions)
    }
}

// Event Handler 
function modalTriggerHandler(item, e) {

    e.preventDefault()

    const template = modalView(csn_editorial.hero, item.getAttribute('data-modal-image-index'))

    //update content
    window.csn_modal.show( template, '_c-modal--slideshow', modalGallery );
}
