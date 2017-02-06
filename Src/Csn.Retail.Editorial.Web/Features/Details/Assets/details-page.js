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

    //Lazy laod the slideshow JS and modal JS
    require.ensure(['../../Shared/Assets/Js/Modules/Slideshow/slideshow.js', '../../Shared/Assets/Js/Modules/Modal/modal.js' ], function() {


        var Slideshow = require('../../Shared/Assets/Js/Modules/Slideshow/slideshow.js').default;

        //Setup slideshow
        Slideshow({
            scope: '[data-slideshow]',
            showPages: false,
            lazyLoad: true,
            infinity: true
        });

        if (document.querySelector('[data-ajax-modal]')) {

            //init Modal JS
            require('../../Shared/Assets/Js/Modules/Modal/modal.js');

            var modalSlideshow = null;

            // load the modal slider after the ajax-event has completed //TODO: not using ajax anymore find better name
            window.addEventListener('ajax-completed', function () {
                //Init modal slideshow
                modalSlideshow = Slideshow({
                    scope: '[data-slideshow-modal]', //this needs to be unique
                    pageBy: 1,
                    showPages: false,
                    lazyLoad: true,
                    infinity: true
                })
            });

            // Resize Handler for modal window to shrink when window hight is less than image
            let addSizeHandlers = function () {

                let modalContent = document.querySelector('._c-modal__content');
                let el = modalContent.querySelector('._c-slideshow__slides');
                var newHeight = window.innerHeight - 80; // TODO: find way to dynamically set spacing

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

            document.querySelector('._c-modal__close').addEventListener('click', function() {
                ['after.csn-slider.lazyload', 'after.csn-slider.resize'].forEach(function(e) {
                    modalInner.removeEventListener(e, addSizeHandlers);
                });
            });
        }

    }, 'details-page--slideshow-and-modal')
}



// Lazy load Editors Rating

function lazyLoadEditorRatings() {
    //TODO: Change the URL to proper ajax request
    let editorRatingsURL = "procon/" + document.querySelector('[name="csn:networkid"]').getAttribute('content')
    let request = new XMLHttpRequest();
    request.open('GET', editorRatingsURL, true);

    request.onreadystatechange = function() {
        if (this.readyState === 4) {
            if (this.status >= 200 && this.status < 400) {
                // Success!
                let resp = this.responseText;
                document.querySelector(".aside").insertAdjacentHTML('beforeend', resp);

                require.ensure(['./Js/editorsRating-component.js'], function() {
                    require('./Js/editorsRating-component.js');
                }, 'editors-ratings')
            } else {
                // Error :(
            }
        }
    };

    request.send();
    request = null;
}

lazyLoadEditorRatings();




