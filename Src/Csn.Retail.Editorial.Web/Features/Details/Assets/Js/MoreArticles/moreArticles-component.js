import Swiper from 'swiper'
import {get} from 'Js/Modules/Ajax/ajax.js'
import moreArticlesView from 'Js/MoreArticles/moreArticles-view.js'

import ScrollMagic from 'ScrollMagic'

const scopeSelector = '.more-articles'
const slideContainer = '.more-articles__slides'
const frame = '.more-articles__frame'
const prevCtrl = '.more-articles__nav-button--prev'
const nextCtrl = '.more-articles__nav-button--next'
const slide = '.more-articles__slide'
const navButtons = '.more-articles__nav-button'
const showHideButton = '.more-articles__button--show-hide'
let userPreference = false

// Scroll Magic
const contentOffset = 0.5; // range 0 - 1
const triggerElement = '.article-type'
const triggerHook = 1
const offset = (document.querySelector(triggerElement).offsetHeight * contentOffset);
window.scrollMogicController = window.scrollMogicController || new ScrollMagic.Controller();


// Init More Articles Slider
let initMoreArticlesSlider = (selector, options) => {
    options = Object.assign({}, options);
    return Swiper(selector, options);
}

// Set text
let setText = (scope, selector, text) => {
    scope.querySelector(selector).innerHTML = text.toString();
}

// add class
let addClass = (scope, selector, className, text) => {
    scope.querySelector(selector).classList.add(className)
    setText(scope, showHideButton, text || className)
}

// remove class
let removeClass = (scope, selector, className, text) => {
    scope.querySelector(selector).classList.remove(className)
    setText(scope, showHideButton, text || className)
}

// Toggle class
let toggleClass = (scope, selector, className, ...text) => {
    if (!userPreference) {
        if (scope.querySelector(selector).classList.contains(className)) {
            removeClass(scope, selector, className, text[0][0])
        } else {
                addClass(scope, selector, className, text[0][1])
        }
    }

}


// Update Button
let updateButton = (scope, selector, attr, data) => {
    if (data) {
        scope.querySelector(selector).setAttribute(attr, data)
    } else {
        scope.querySelector(selector).removeAttribute(attr)

    }
}

// Add event listener to Button
let addEventListenerToButton = (scope, selector, event, cb, cbArgs) => {
    let list = scope.querySelectorAll(selector);
    for (var i of list) {
        i.addEventListener(event, (e) => {
            cb(e, cbArgs)
        })
    }
}

// Add event listener to Button
let removeEventListenerToButton = (scope, selector, event, fn) => {
    let list = scope.querySelectorAll(selector);
    for (var i of list) {
        i.removeEventListener(event, fn, false)
    }
}

// Update List
let updateList = (scope, selector, data) => {
    scope.querySelector(selector).insertAdjacentHTML('beforeend', moreArticlesView(data))
}

// Update Content
let updateContent = function(scope, selector, ajax, container, cb) {

    const el = scope.querySelector(selector)
    const query = el ? el.getAttribute('data-more-articles-query'): null;
    const url = el ? el.getAttribute('data-more-articles-path') + query: null;
    let lock = !!scope.querySelector(nextCtrl).getAttribute('data-disabled')

    if (!lock && query) {
        updateButton(scope, nextCtrl, 'data-disabled', 1)//Prevent multiple requests
        scope.querySelector(frame).classList.add('loading')
        ajax(url, (json) => {
            json = JSON.parse(json)
            if (json.NextQuery) {
                updateButton(scope, nextCtrl, 'data-more-articles-query', json.NextQuery)
            } else {
                //disabled next
                updateButton(scope, nextCtrl, 'data-more-articles-query', '')
            }
            updateButton(scope, nextCtrl, 'data-disabled')
            scope.querySelector(frame).classList.remove('loading')
            updateList(scope, container, json)
            cb()
        })
    }

}

// Get slider length
let slidesLength = (scope, selector) => {
    return scope.querySelectorAll(selector).length
}

// handle filter active class

let filterHandler = (e, ...args) => {
    e.preventDefault();

    //check if alread active
    const el = e.target
    const slider = args[0][0]
    const scope = args[0][1]
    const className = args[0][2]

    if(!el.classList.contains(className)) {

        // Clear all active classes and add to clicked el
        const filters = scope.querySelectorAll(el.className)
        for (var filter of filters) {
            filter.classList.remove(className)
        }
        el.classList.add(className)

        //get url and set it to next
        updateButton(scope, nextCtrl, 'data-more-articles-query', el.pathname)
        //destory old slider
        scope.querySelector(slideContainer).innerHTML = '';
        //Init new slider
        updateContent(
            scope,
            nextCtrl,
            get,
            slideContainer,
            () => {
                slider.slideTo(0)
                updateButton(scope, prevCtrl, 'disabled', 'true')
                slider.update();
            }
        )
    }

    if (!scope.classList.contains('show')) {
        toggleClass(document, scopeSelector, 'show', ['Show', 'Hide'])
    }
}


// handlers
function buttonHandler(scope, slider, firstSlide, visibleSlides) {
    // Prev logic
    if (slider.activeIndex <= firstSlide) {
        updateButton(scope, prevCtrl, 'disabled', 'true')
    } else {
        updateButton(scope, prevCtrl, 'disabled')
    }

    //Next logic
    if (slider.activeIndex + visibleSlides >= slidesLength(scope, slide)) {
        updateButton(scope, nextCtrl, 'disabled', 'true')
    } else {
        updateButton(scope, nextCtrl, 'disabled')
    }
}

function nextButtonHandler(scope, slider, offset, cb) {
    // Get slider index and check if offset from end
    if (slider.activeIndex >= slidesLength(scope, slide) - offset) {
        cb()
    }
}

function buttonShowHideHandler() {
    //set user prefernce here
    if (document.querySelector(scopeSelector).classList.contains('show')) {
        toggleClass(document, scopeSelector, 'show', ['Show', 'Hide'])
        userPreference = true
    } else if (userPreference) {
        userPreference = false
        toggleClass(document, scopeSelector, 'show', ['Show', 'Hide'])
    }

}

// Filters
let filters = (scope, selector, cb, cbArgs) => {
    addEventListenerToButton(scope, selector, 'click', cb, cbArgs)
}


//Scroll Magic

let scrollHandler = (scope, selector, className) => {
    const el = scope.querySelector(selector)
    //if more articles is already active then don't
    if (el.classList.contains('active') && !el.classList.contains(className)) {
        toggleClass(document, scopeSelector, className, ['Show', 'Hide'])
    }
}

// Set scene
new ScrollMagic.Scene({
    triggerElement: triggerElement,
    triggerHook: triggerHook,
    offset: offset
})
    .on("update", function (e) {
        e.target.controller().info("scrollDirection") === 'REVERSE' ? this.trigger("enter") : null;
    })
    .on("enter", scrollHandler.bind(null, document, scopeSelector, 'show'))
    .addTo(window.scrollMogicController);




// Main
let main = (scope) => {

    // Init Slider

    const firstSlide = 0
    const visibleSlides = 3;
    const offset = 2 + visibleSlides

    let options = {

        // Optional parameters
        slidesPerView: 3,
        slidesPerGroup: 1,

        // Navigation arrows
        nextButton: '.more-articles__nav-button--next',
        prevButton: '.more-articles__nav-button--prev',

        //Namespace
        wrapperClass: 'more-articles__slides',
        slideClass: 'more-articles__slide',
    }
    const slider = initMoreArticlesSlider(frame, options);

    window.slider = slider

    const content = () => { return updateContent(scope, nextCtrl, get, slideContainer, () => {
        slider.update();
    })}

    // Init
    updateButton(scope, prevCtrl, 'disabled', 'true')

    // Init Button Handlers
    addEventListenerToButton(scope, navButtons, 'click', buttonHandler.bind(null, scope, slider, firstSlide, visibleSlides))

    // Init next Button
    addEventListenerToButton(scope, nextCtrl, 'click', nextButtonHandler.bind(null, scope, slider, offset, content))

    //Init Filters
    filters(scope, '.more-articles__filter', filterHandler, [slider, scope, 'active'])

    // Init show/hide Button
    addEventListenerToButton(scope, showHideButton, 'click', buttonShowHideHandler)

}

// Call this first to make sure there is content for the slider or it will bug out
updateContent(
    document.querySelector(scopeSelector),
    nextCtrl,
    get,
    slideContainer,
    () => {
        main(document.querySelector(scopeSelector))
        // This is called in cb() if AJAX is sucessful on first time
        document.querySelector(scopeSelector).classList.add('active');
        }
    )



