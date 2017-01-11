// Details Page css files

require('./css/Details-page.scss');

// APP

// If Slideshow then must have Modal
 if (document.querySelector('[data-slideshow]')) {
    require.ensure(['../../Shared/Assets/Js/Modules/Slideshow/slideshow.js', '../../Shared/Assets/Js/Modules/Modal/modal.js'], function () {

        var Slideshow = require('../../Shared/Assets/Js/Modules/Slideshow/slideshow.js');
            Slideshow({
                scope: '[data-slideshow]',
                showPages: false,
                lazyLoad: true,
                infinity: true
            });
        if(document.querySelector('[data-ajax-modal]'))
            {
            var Modal = require('../../Shared/Assets/Js/Modules/Modal/modal.js');

            window.addEventListener('ajax-completed', function (e) {
                Slideshow({
                    scope: '[data-slideshow-modal]',
                    pageBy: 1,
                    showPages: false,
                    lazyLoad: true,
                    infinity: true
                })

            });
            }


    })

}



if (module.hot) {
    module.hot.accept()
}