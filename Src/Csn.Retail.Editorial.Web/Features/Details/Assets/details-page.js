// Details Page css files
require('./css/details-page.scss');

//------------------------------------------------------------------------------------------------------------------

import "core-js/shim";

// Dynamically set the public path for ajax/code-split requests
let scripts = document.getElementsByTagName("script");
let scriptsLength = scripts.length;
let patt = /csn\.common/;
for (var i = 0; i < scriptsLength; i++) {
    var str = scripts[i].getAttribute('src');
    if (patt.test(str)) {
        __webpack_public_path__ = str.substring(0, str.lastIndexOf("/")) + '/';
        break;
    }
}

//------------------------------------------------------------------------------------------------------------------


// APP
let aboveTheFold = function() {
    // If Slideshow then must have Modal
    if (document.querySelector('[data-slideshow]')) {

        // Correct the size when the brightcove video is smaller than
        if (document.querySelector('.hero .brightcove__iframe-wrapper') && document.querySelector('._c-slideshow__slides')) {
            window.addEventListener('after.csn-slider.lazyload', function() {
                document.querySelector('.hero .brightcove__iframe-wrapper').style.paddingTop = document.querySelector('._c-slideshow__slides').offsetHeight + "px"
                window.addEventListener('resize', () => {
                    document.querySelector('.hero .brightcove__iframe-wrapper').style.paddingTop = document.querySelector('._c-slideshow__slides').offsetHeight + "px"
                })
            })
        }

        //Lazy load - Above the fold because some detail layouts don't need it
        require.ensure([
                "Js/Modules/Slideshow/slideshow.js", "Js/Modules/Modal/modal.js"
        ],
            function() {

                var Slideshow = require("Js/Modules/Slideshow/slideshow.js").default;

                //Setup slideshow
                Slideshow({
                    scope: '[data-slideshow]',
                    showPages: false,
                    lazyLoad: true,
                    infinity: true
                });

                if (document.querySelector("[data-ajax-modal]")) {

                    //init Modal JS
                    require("Js/Modules/Modal/modal.js");

                    var modalSlideshow = null;

                    // load the modal slider after the ajax-event has completed //TODO: not using ajax anymore find better name
                    window.addEventListener('ajax-completed',
                        function() {
                            //Init modal slideshow
                            modalSlideshow = Slideshow({
                                scope: '[data-slideshow-modal]', //this needs to be unique
                                pageBy: 1,
                                showPages: false,
                                lazyLoad: true,
                                infinity: true
                            })
                        });

                    // Resize Handler for modal window to shrink when window height is less than image
                    let addSizeHandlers = function() {

                        let modalContent = document.querySelector('._c-modal__content');
                        let el = modalContent.querySelector('._c-slideshow__slides');
                        let threshold = 80; // TODO: find way to dynamically set spacing
                        let windowHeight = window.innerHeight - threshold;

                        // Compare image dimensions with window
                        if (windowHeight < el.offsetHeight) {

                            let imageRatio = 3 / 2;
                            let width = Math.round((imageRatio * windowHeight));

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

                }

            },
            'details-page--slideshow-and-modal');
    }
}
aboveTheFold();

//Editors Rating
let editorRatings = function() {
    if (document.querySelector('.editors-ratings')) {
        require('./Js/editorsRating-component.js');
    }
}
editorRatings();

// Lazy load Media Motive
let mediaMotive = function () {

    require.ensure(['Js/Modules/MediaMotive/mm.js'],
        function() {
            require('Js/Modules/MediaMotive/mm.js');
        },
        'Media-Motive');
}
window.addEventListener("load", function load(event){
    window.removeEventListener("load", load, false); //remove listener, no longer needed
    mediaMotive();
},false);

// TEADS
$(function () {
    if ($('#teads-video-container').length) {
        $('#teads-video-container').insertAfter($('.article__copy p:eq(1)')).wrap('<p></p>');

    }
});

//Lazy load More articles JS

let moreArticles = function() {
    require.ensure(['./Js/moreArticles-component.js'],
    function() {
        require('./Js/moreArticles-component.js');
    },
    'More-Articles-Component');
}



