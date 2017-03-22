import 'Css/Modules/Widgets/SpecModule/_specModule.scss'

import Swiper from 'swiper'
import * as Ajax from 'Js/Modules/Ajax/ajax.js'
import * as View from 'Js/Modules/SpecModule/specModule-view.js'

// Get Scope and cache selectors
const scope = document.querySelector('.spec-module-placeholder');


// Init More Articles Slider
let initMoreArticlesSlider = (selector, options) => {
    options = Object.assign({}, options);
    return Swiper(selector, options);
}

// Add event listener to Button
const addEventListenerToButton = (el, event, cb, cbArgs) => {

    if (typeof el.length === 'undefined') {
        el.addEventListener(event, (e) => {
            cb(e, cbArgs)
        }
        )
    } else {
        for (var i of el) {

            i.addEventListener(event, (e) => {
                cb(e, cbArgs)
            }
            )
        }
    }
}

const getAttribute = (el, attr) => {
    return el.getAttribute(attr)
}

const buildQuery = (evt, indexAttr, slider, attr) => {

    //get index of pagination bullet
    const index = getAttribute(evt.target, indexAttr) 
    //use index to get equivalent slide 
    const slide = slider.slides[index];

    //Check if slide has content, if so return false
    if (slide.innerHTML.trim() !== "") {
        return false
    } else {
        return {
            slide: slide,
            url: getAttribute(slide, attr)
        }  
    }

}

// Make Query - Ajax
const makeQuery = (url, el, cb = () => { }, onError = () => { }) => {

    //Make Query
    Ajax.get(url,
        (resp) => {
            //update list
            cb(resp, el)
        },
        () => {
            onError()
        }
    )
}

let init = (scope, data) => {

    //Render Container
    scope.innerHTML = View.container(data);

    let sliderContainer = scope.querySelector('.spec-module .slideshow__container')

    // Init Swiper
    let options = {

        // Optional parameters
        slidesPerView: 1,
        slidesPerGroup: 1,

        //Namespace
        wrapperClass: 'slideshow__slides',
        slideClass: 'slideshow__slide',
        lazyStatusLoadingClass: 'slideshow__image--loading',
        lazyStatusLoadedClass: 'slideshow__image--loaded',
        lazyPreloaderClass: 'slideshow__image--preloader',

        //Pagination
        pagination: '.slideshow__pagination',
        paginationType: 'bullets',
        paginationHide: false,
        paginationClickable: true,
        paginationBulletRender: function (swiper, index, className) {
            return '<span class="' + className + '" data-slide-index="' + index + '"></span>';
        }
    }
    const slider = initMoreArticlesSlider(sliderContainer, options);
    const sliderBullets = [];
    slider.bullets.each(function(i, el) { sliderBullets.push(el) });

    addEventListenerToButton(
        sliderBullets,
        'click',
        (e) => {
            const item = buildQuery(e, 'data-slide-index', slider, 'data-spec-url')
            if (item) {
                makeQuery(item.url, item.slide,
                    (resp, el) => {
                        console.log(resp)
                        el.innerHTML = View.item(JSON.parse(resp))
                    }
                )  
            }
        }
    )

 
}
init(scope, csn_editorial.specModule)