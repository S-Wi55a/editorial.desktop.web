export default function() {
    const multipleImageLayout = document.querySelector('.hero--multipleImages')
    const imageAndVideoLayout = document.querySelector('.hero--imageAndVideo')
    const singleImageLayout = document.querySelector('.hero--singleImage')
    const doubleImageLayout = document.querySelector('.hero--doubleImage')


    if (multipleImageLayout || imageAndVideoLayout || singleImageLayout || doubleImageLayout) {
        require.ensure(['swiper', 'Js/Modules/Modal/modal.js', 'Js/Modules/Hero/hero-view--modal.js'],
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
                    1199: {
                        slidesPerView: 1,
                    }
                }
            })
        } else if (doubleImageLayout) {
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


        let modalContainer = document.querySelector('._c-modal');
        if (modalContainer) {

            //init Modal JS
            const Modal = require('Js/Modules/Modal/modal.js').default;

            window.csn_modal = window.csn_modal || new Modal()

            const View = require('Js/Modules/Hero/hero-view--modal.js');
            const scope = document.querySelector('.hero');
            //Add event listeners to hero content
            const modalTrigger = scope.querySelectorAll('[data-modal-trigger]');
            for (let item of modalTrigger) {
                item.addEventListener('click', function (e) {
                    e.preventDefault();
                    //update content
                    window.csn_modal.show(
                        View.template(csn_editorial.hero, item.getAttribute('data-modal-image-index')),
                        '_c-modal--slideshow',
                        function () {

                            // This is for GA Gallery tracking requested by the BI team
                            if (CsnInsightsEventTracker) {
                                const eventContextCopy = Object.assign({}, eventContext.metaData);
                                eventContextCopy.metaData.ContentGroup2 = 'gallery';
                                CsnInsightsEventTracker.sendPageView(eventContextCopy.metaData);
                            }


                            const initialSlide = !!document.querySelector('.slideshow--modal') ? parseInt(document.querySelector('.slideshow--modal').getAttribute('data-slideshow-start')) : 0
                            const modalSwiper = new Swiper('._c-modal .slideshow', {

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
                                onInit: (swiper) => {

                                    // Find the first image to be displayed
                                    const _img = swiper.slides[swiper.activeIndex].querySelector('.slideshow__image')

                                    const src = _img.getAttribute('data-src'),
                                        srcset = _img.getAttribute('data-srcset'),
                                        sizes = _img.getAttribute('data-sizes')

                                    // Once image is loaded, use callback
                                    swiper.loadImage(_img, src, srcset, sizes, false, () => {
                                        if (typeof swiper === 'undefined' || swiper === null || !swiper) return;
                                        else {
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

                                            // Set dimenions for wrapper and clear transtions
                                            resizeHandler(swiper, _img)
                                            swiper.slideTo(swiper.activeIndex)

                                        }
                                    })
                                }
                                
                            })

                            // Handler to set the dimensions of the Swiper wrapper
                            function resizeHandler(swiper, img) {

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

                            // Init setup
                            modalSwiper.wrapper[0].style.width = 0;

                            modalSwiper.on('onTransitionStart', (swiper) => {
                                swiper.wrapper[0].classList.add('swiper-transition')
                                resizeHandler(swiper) // Set size of wrapper
                            })

                            modalSwiper.on('onTransitionEnd', (swiper) => {
                                swiper.onResize() // Reset state of swiper 
                                swiper.wrapper[0].classList.remove('swiper-transition')
                            })

                            // handle event
                            window.addEventListener('resize', resizeHandler.bind(null, modalSwiper, null ))

                            window.addEventListener('modal.close', function () {
                                window.removeEventListener('resize', resizeHandler)
                            })

                        }
                        );
                });
            }

        }
    },
    'Hero Slidier');
    }
}