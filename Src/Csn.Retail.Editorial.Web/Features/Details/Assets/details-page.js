// Details Page css files

require('./css/Details-page.scss');

// APP


// Check for Slideshow
if (document.querySelector('[data-slideshow]')) {
    require.ensure(['../../Shared/Assets/Js/Modules/Slideshow/slideshow.js'], function() {

        var Slideshow = require('../../Shared/Assets/Js/Modules/Slideshow/slideshow.js');
            Slideshow({
                scope: '[data-slideshow]',
                showPages: false,
                lazyLoad: true
            });
    })
}

// Check for Modal
if (document.querySelector('._c-modal')) {
    require.ensure(['../../Shared/Assets/Js/Modules/Slideshow/slideshow.js',
                    '../../Shared/Assets/Js/Modules/Modal/modal.js'],
                    function () {

        var Modal = require('../../Shared/Assets/Js/Modules/Modal/modal.js');



        var Slideshow = require('../../Shared/Assets/Js/Modules/Slideshow/slideshow.js');
        window.addEventListener('ajax-completed', function (e) {
            Slideshow({
                scope: '[data-slideshow-modal]',
                pageBy: 1,
                showPages: false,
            })

        });
    })

}



if (module.hot) {
    module.hot.accept()
}