// Details Page css files

require('./css/Details-page.scss');

// APP



// If Slideshow then must have Modal
if (document.querySelector('[data-slideshow]')) {

    // Correct the size when the brightcove video is smaller than
    if (document
        .querySelector('.hero .brightcove__iframe-wrapper') &&
        document.querySelector('._c-slideshow__slides')) {
        window.addEventListener('after.csn-slider.lazyload',
            function() {
                document.querySelector('.hero .brightcove__iframe-wrapper').style
                    .paddingTop = document.querySelector('._c-slideshow__slides').offsetHeight + "px"
                window.addEventListener('resize',
                    () => {
                        document.querySelector('.hero .brightcove__iframe-wrapper').style
                            .paddingTop = document.querySelector('._c-slideshow__slides').offsetHeight + "px"
                    })
            })
    }

    require.ensure([
            '../../Shared/Assets/Js/Modules/Slideshow/slideshow.js', '../../Shared/Assets/Js/Modules/Modal/modal.js'
        ],
        function() {

            var Slideshow = require('../../Shared/Assets/Js/Modules/Slideshow/slideshow.js');
            Slideshow({
                scope: '[data-slideshow]',
                showPages: false,
                lazyLoad: true,
                infinity: true
            });
            if (document.querySelector('[data-ajax-modal]')) {

                var Modal = require('../../Shared/Assets/Js/Modules/Modal/modal.js');
                var modalSlideshow = null;

                window.addEventListener('ajax-completed',
                    function (e) {

                        modalSlideshow = Slideshow({
                            scope: '[data-slideshow-modal]',
                            pageBy: 1,
                            showPages: false,
                            lazyLoad: true,
                            infinity: true
                        })

                    });


                let addSizeHandlers = function () {


                    let modalContent = document.querySelector('._c-modal__content');
                    let el = modalContent.querySelector('._c-slideshow__slides');
                    var newHeight = window.innerHeight - 80;

                    if (newHeight < el.offsetHeight) {

                        let imageRatio = 3/2

                        var width = Math.round((imageRatio * newHeight));

                        el.style.width = (Math.round(width)) + 'px';

                        //then force slider re position
                        el.style.transform = 'translate3d(-' + width * modalSlideshow.currentSlide() + 'px,0,0)';
                        el.style.webkitTransform = 'translate3d(-' + width * modalSlideshow.currentSlide() + 'px,0,0)';

                    }

                }

                let modalInner = document.querySelector('._c-modal__inner');

                ['after.csn-slider.lazyload', 'after.csn-slider.resize'].forEach(function(e) {
                    modalInner.addEventListener(e, addSizeHandlers);
                });

                document.querySelector('._c-modal__close').addEventListener('click',
                    function() {
                        ['after.csn-slider.lazyload', 'after.csn-slider.resize'].forEach(function(e) {
                            modalInner.removeEventListener(e, addSizeHandlers);
                        });
                    });


            }

        })
}


if (module.hot) {
    module.hot.accept()
}