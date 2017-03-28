import 'Css/Modules/Widgets/SpecModule/_specModule.scss'

import Swiper from 'swiper'
import * as Ajax from 'Js/Modules/Ajax/ajax.js'
import * as View from 'Js/Modules/SpecModule/specModule-view.js'
import Modal from 'Js/Modules/Modal/modal.js'

window.csn_modal = window.csn_modal || new Modal()


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
        for (let i of el) {

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

// Pagination handler
const paginationHandler = (container, bullets, min, max, text) => {

    //Cache el properties
    const maxText = max.innerHTML
    const minText = min.innerHTML

    let getSize = (element) => {
        // we're assuming a reference to your element in a variable called 'element'
        var style = element.currentStyle || window.getComputedStyle(element),
            width = element.offsetWidth, // or use style.width
            margin = parseFloat(style.marginLeft) + parseFloat(style.marginRight),
            padding = parseFloat(style.paddingLeft) + parseFloat(style.paddingRight),
            border = parseFloat(style.borderLeftWidth) + parseFloat(style.borderRightWidth);

        //if the styles aren't loaded yet then set default size 
        if ((width + margin - padding + border) === 0) {
            return 15*3
        } else {
            return (width + margin - padding + border);            
        }

    }

    const bulletLength = bullets.length
    const limit = 10
    const pages = bulletLength / limit
    let currentPage = 0
    let pageWidth = getSize(bullets[0]) * limit
    const bar = container

    bar.style.width = getSize(bullets[0]) * bulletLength + 'px';

    //Asses how many bulets there are
    if (bulletLength > limit) {
        //if greater than then add more button
        max.innerHTML = text;
        max.addEventListener('click', () => {
            console.log('currentPage: ', currentPage, 'Pages: ', pages)
            if (currentPage < Math.floor(pages)) {
                currentPage += 1;
                bar.style.WebkitTransform = 'translate3d(-' + currentPage * pageWidth + 'px,0,0)';
                min.innerHTML = text;
                if (currentPage >= Math.floor(pages)) {
                    max.innerHTML = maxText;
                }
            }

        })

        min.addEventListener('click', () => {
            console.log('currentPage: ', currentPage)

            if (currentPage > 0) {
                currentPage -= 1;
                bar.style.WebkitTransform = 'translate3d(-' + currentPage * pageWidth + 'px,0,0)';
                max.innerHTML = text;

                if (currentPage <= 0) {
                    currentPage = 0;
                    min.innerHTML = minText;
                }
            }

        })
    }

}

// Data disclaimer handler
const disclaimerHandler = (el, attr) => {

    const content = decodeURI(getAttribute(el, attr))
    window.csn_modal.show(View.disclaimer(content))

}

// Toggle class
const toggleClass = (el, className) => {
    if (el.classList.contains(className)) {
        el.classList.remove(className)
    } else {
        el.classList.add(className)
    }
}

let init = (scope, data) => {

    //Render Container
    scope.innerHTML = View.container(data);

    scope = scope.querySelector('.spec-module')
    const sliderContainer = scope.querySelector('.spec-module .slideshow__container')
    const loadingClass = 'loading'
    const nextButton = scope.querySelector('.slideshow__pagination-label--max')
    const prevButton = scope.querySelector('.slideshow__pagination-label--min')
    const paginationBar = scope.querySelector('.slideshow__pagination')


    // Init Swiper
    const options = {

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
                toggleClass(scope, loadingClass)
                makeQuery(item.url, item.slide,
                    (resp, el) => {
                        el.innerHTML = View.item(JSON.parse(resp))
                        toggleClass(scope, loadingClass)
                        //Add Handler for disclaimer
                        const modalTrigger = scope.querySelectorAll('[data-disclaimer]');
                        for (let item of modalTrigger) {
                            item.addEventListener('click', disclaimerHandler.bind(null, item, 'data-disclaimer'));
                        }
                    }
                )  
            }
        }
    )

    //trigger first item click
    sliderBullets[0].click()

    // Add handlers for pagination
    paginationHandler(paginationBar, sliderBullets, prevButton, nextButton, 'More')

}
init(scope, csn_editorial.specModule)