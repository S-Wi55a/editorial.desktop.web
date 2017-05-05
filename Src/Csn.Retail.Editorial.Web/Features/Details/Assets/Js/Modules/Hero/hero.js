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
                            CsnInsightsEventTracker.sendPageView(eventContext.galleryMetaData);

                            const initSlide = !!document.querySelector('.slideshow--modal') ? parseInt(document.querySelector('.slideshow--modal').getAttribute('data-slideshow-start')) : 0
                            const modalSwiper = new Swiper('._c-modal .slideshow', {

                                initialSlide: initSlide,
                                roundLengths: true,

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
                                paginationHide: false

                            })

                            window.modalSwiper = modalSwiper

                            let resizeHandler = (swiper) => {

                                const img = swiper.slides[swiper.activeIndex].querySelector('.slideshow__image')
                                const w = swiper.slides[swiper.activeIndex].querySelector('.slideshow__image').width
                                const h = swiper.slides[swiper.activeIndex].querySelector('.slideshow__image').height

                                let windowHeight = window.innerHeight - 80; //TODO - change to dynamic or var
                                let windowWidth = window.innerWidth - 160; //TODO - change to dynamic or var

                                let windowRatio = windowWidth / windowHeight;

                                let imageRatio = img.naturalWidth / img.naturalHeight;

                                console.log('windowRatio: ', windowRatio, 'imageRatio: ', imageRatio, windowRatio/imageRatio )

                                let compareRatio = windowRatio / imageRatio

                                //imageRatio > 1 = wider
                                //imageRatio < 1 = taller


                                if (windowRatio < imageRatio) {

                                        let width = windowWidth;
                                        let height = Math.round((width / imageRatio));

                                        console.log('w', imageRatio, windowWidth, height)

                                        if (img.naturalWidth > width) {
                                            swiper.wrapper[0].style.width = (Math.round(width)) + 'px';
                                            swiper.wrapper[0].style.height = (Math.round(height)) + 'px';
                                        } else {
                                            swiper.wrapper[0].style.width = img.naturalWidth + 'px';
                                            swiper.wrapper[0].style.height = img.naturalHeight + 'px';
                                            swiper.onResize();
                                        }
                                    
                                        //then force slider re-position
                                        swiper.onResize()
                                } else if (windowRatio > imageRatio) {

                                        let width = Math.round((imageRatio * windowHeight));

                                        let height = Math.round((width / imageRatio));

                                        console.log('h', imageRatio, width, height)

                                        if (img.naturalHeight > height) {
                                            swiper.wrapper[0].style.width = (Math.round(width)) + 'px';
                                            swiper.wrapper[0].style.height = (Math.round(height)) + 'px';
                                        } else {
                                            swiper.wrapper[0].style.width = img.naturalWidth + 'px';
                                            swiper.wrapper[0].style.height = img.naturalHeight + 'px';
                                            swiper.onResize();
                                        }

                                        //then force slider re-position
                                        swiper.onResize()
                                }

                            }

                            // Call it once to get appropriate size on first view
                            modalSwiper.once('lazyImageReady', (swiper, slide, image) => {
                                swiper.onResize();
                            })

                            modalSwiper.on('lazyImageReady', (swiper, slide, image) => {
                                resizeHandler(swiper)
                            })

                            modalSwiper.on('click', (swiper) => {
                                resizeHandler(swiper)
                            })

                            // handle event
                            window.addEventListener('resize', resizeHandler.bind(null, modalSwiper))

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