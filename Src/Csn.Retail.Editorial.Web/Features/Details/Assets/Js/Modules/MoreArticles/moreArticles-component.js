import Swiper from 'swiper'
import * as Ajax from 'Js/Modules/Ajax/ajax.js'
import * as View from 'Js/Modules/MoreArticles/moreArticles-view.js'

import ScrollMagic from 'ScrollMagic'

let userPreference = false
let showText = csn_editorial.moreArticles.headings.showHeading
let hideText = csn_editorial.moreArticles.headings.hideHeading

const customEvent = new Event('csn_editorial.moreArticles.ready');

// Init More Articles Slider
let initMoreArticlesSlider = (selector, options) => {
    options = Object.assign({}, options);
    return Swiper(selector, options);
}

// Set text
let setText = (el, text) => {
    if (text) {
        el.innerHTML = text.toString();
    }
}

// add class
let addClass = (el, className, cb) => {
    el.classList.add(className)
    if (typeof cb === "function") {
        cb()
    }
}

// remove class
let removeClass = (el, className, cb) => {
    el.classList.remove(className)
    if (typeof cb === "function") {
        cb()
    }
}

// Toggle class
let toggleClass = (el, className, ...cb) => {
    if (!userPreference) {
        if (el.classList.contains(className)) {
            removeClass(el, className, cb[0][0])
        } else {
            addClass(el, className, cb[0][1])
        }
    }
}

// Update Button
let updateButton = (el, attr, data) => {
    if (data) {
        el.setAttribute(attr, data)
    } else {
        el.removeAttribute(attr)
    }
}

// Add event listener to Button
let addEventListenerToButton = (el, event, cb, cbArgs) => {

    if (typeof el.length === 'undefined') {
        el.addEventListener(event, (e) => {
            cb(e, cbArgs)
        })
    } else {
        for (let i of el) {
            i.addEventListener(event, (e) => {
                cb(e, cbArgs)
            })
        }
    }
}

// Update List
let updateList = (el, data) => {
    el.insertAdjacentHTML('beforeend', View.article(data))
}

// Update Content
let updateContent = function(frame, el, container, cb) {

    const query = el ? el.getAttribute('data-more-articles-query') : null;
    const url = el ? el.getAttribute('data-more-articles-path') + query : null;
    let lock = !!el.getAttribute('data-disabled')

    if (!lock && query) {
        updateButton(el, 'data-disabled', 1) //Prevent multiple requests
        frame.classList.add('loading')
        Ajax.get(url, (json) => {
            json = JSON.parse(json)
            if (json.nextQuery) {
                updateButton(el, 'data-more-articles-query', json.nextQuery)
            } else {
                //disabled next
                updateButton(el, 'data-more-articles-query', '')
            }
            updateButton(el, 'data-disabled')
            frame.classList.remove('loading')
            updateList(container, json)
            cb()
        })
    }

}


// handle filter active class
let filterHandler = (e, ...args) => {
    e.preventDefault();

    //check if alread active
    const el = e.target
    const slider = args[0][0]
    const scope = args[0][1]
    const className = args[0][2]
    const classNameLast = args[0][3]
    userPreference = true // assuming filter clicked is active


    if (el.classList.contains(classNameLast)) {
        window.location.href = el.href;
    }

    if (!el.classList.contains(className)) {

        userPreference = false

        // Clear all active classes and add to clicked el
        const filters = scope.self.querySelectorAll(el.className)
        for (var filter of filters) {
            filter.classList.remove(className)
        }
        el.classList.add(className)

        //get url and set it to next
        updateButton(scope.moreArticlesNextCtrl, 'data-more-articles-query', el.pathname)
            //destory old slider
        scope.moreArticlesSlideContainer.innerHTML = '';
        //Init new slider
        updateContent(
            scope.moreArticlesFrame,
            scope.moreArticlesNextCtrl,
            scope.moreArticlesSlideContainer,
            () => {
                slider.slideTo(0)
                updateButton(scope.moreArticlesPrevCtrl, 'disabled', 'true')
                slider.update();
                window.dispatchEvent(customEvent)
            }
        )
    }

    if (!scope.self.classList.contains('show')) {
        toggleClass(scope.self, 'show', [setText.bind(null, scope.moreArticlesShowHideButton, showText), setText.bind(null, scope.moreArticlesShowHideButton, hideText)])
    }
}


// handlers
let buttonHandler = (scope, slider, firstSlide, visibleSlides) => {
    // Prev logic
    if (slider.activeIndex <= firstSlide) {
        updateButton(scope.moreArticlesPrevCtrl, 'disabled', 'true')
    } else {
        updateButton(scope.moreArticlesPrevCtrl, 'disabled')
    }

    //Next logic
    if (slider.activeIndex + visibleSlides >= slider.slides.length) {
        updateButton(scope.moreArticlesNextCtrl, 'disabled', 'true')
    } else {
        updateButton(scope.moreArticlesNextCtrl, 'disabled')
    }
}

let nextButtonHandler = (slider, offset, cb) => {
    // Get slider index and check if offset from end
    if (slider.activeIndex >= slider.slides.length - offset) {
        cb()
    }
}

let buttonShowHideHandler = (scope) => {
    //set user prefernce here
    if (scope.self.classList.contains('show')) {
        toggleClass(scope.self, 'show', [setText.bind(null, scope.moreArticlesShowHideButton, showText), setText.bind(null, scope.moreArticlesShowHideButton, hideText)])
        userPreference = true
    } else if (userPreference) {
        userPreference = false
        toggleClass(scope.self, 'show', [setText.bind(null, scope.moreArticlesShowHideButton, showText), setText.bind(null, scope.moreArticlesShowHideButton, hideText)])
    } else {
        toggleClass(scope.self, 'show', [setText.bind(null, scope.moreArticlesShowHideButton, showText), setText.bind(null, scope.moreArticlesShowHideButton, hideText)])
    }

}

// Filters
let filters = (el, cb, cbArgs) => {
    addEventListenerToButton(el, 'click', cb, cbArgs)
}

//Scroll Magic
let scrollMagic = (scope) => {

    // Scroll Magic
    const contentOffset = 0.5; // range 0 - 1
    const triggerElement = '.article-type'
    const triggerHook = 1
    const offset = (document.querySelector(triggerElement).offsetHeight * contentOffset);
    window.scrollMogicController = window.scrollMogicController || new ScrollMagic.Controller();

    let scrollHandler = (el, className, classNameArr) => {
        //if more articles is already active then don't
        if (el.classList.contains('active') && !el.classList.contains(className)) {
            toggleClass(el, className, classNameArr)
        }
    }

    // Set scene
    let moreArticleScene1 = new ScrollMagic.Scene({
            triggerElement: triggerElement,
            triggerHook: 0,
            reverse: false
        })
        .on("enter", toggleClass.bind(null, scope.self, 'ready'))
        .addTo(window.scrollMogicController);

    let moreArticleScene2 = new ScrollMagic.Scene({
            triggerElement: triggerElement,
            triggerHook: triggerHook,
            offset: offset
        })
        .on("update",
            function(e) {
                e.target.controller().info("scrollDirection") === 'REVERSE' && !userPreference ?
                    this.trigger("enter") :
                    null;
            })
        .on("enter", scrollHandler.bind(null, scope.self, 'show', [setText.bind(null, scope.moreArticlesShowHideButton, showText), setText.bind(null, scope.moreArticlesShowHideButton, hideText)]))
        .addTo(window.scrollMogicController);
}

// Main
let main = (scope = {}) => {

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
    const slider = initMoreArticlesSlider(scope.moreArticlesFrame, options);

    const content = () => {
        return updateContent(
            scope.moreArticlesFrame,
            scope.moreArticlesNextCtrl,
            scope.moreArticlesSlideContainer,
            () => {
                slider.update();
                window.dispatchEvent(customEvent)
            }
        )
    }

    //Init Button Handlers
    addEventListenerToButton(
        scope.moreArticlesNavButtons,
        'click',
        buttonHandler.bind(null, scope, slider, firstSlide, visibleSlides)
    )

    // Init next Button
    addEventListenerToButton
        (scope.moreArticlesNextCtrl,
            'click',
            nextButtonHandler.bind(null, slider, offset, content)
        )

    //Init Filters
    filters(scope.moreArticlesFilter, filterHandler, [slider, scope, 'more-articles__filter--active', 'more-articles__filter--last'])

    // Init show/hide Button
    addEventListenerToButton(scope.moreArticlesShowHideButton, 'click', buttonShowHideHandler.bind(null, scope))

}



const init = (scopeSelector, data) => {

    const scope = {
            self: scopeSelector
        }
        //render container
    scope.self.innerHTML = View.container(data)

    // Cache selectors
    scope.self = scope.self.querySelector('.more-articles')
    scope.moreArticlesSlideContainer = scope.self.querySelector('.more-articles__slides')
    scope.moreArticlesFrame = scope.self.querySelector('.more-articles__frame')
    scope.moreArticlesPrevCtrl = scope.self.querySelector('.more-articles__nav-button--prev')
    scope.moreArticlesNextCtrl = scope.self.querySelector('.more-articles__nav-button--next')
    scope.moreArticlesSlide = scope.self.querySelector('.more-articles__slide')
    scope.moreArticlesNavButtons = scope.self.querySelectorAll('.more-articles__nav-button')
    scope.moreArticlesShowHideButton = scope.self.querySelector('.more-articles__button--show-hide')
    scope.moreArticlesFilter = scope.self.querySelectorAll('.more-articles__filter')

    //Scroll Magic
    scrollMagic(scope)

    updateContent(
        scope.moreArticlesFrame,
        scope.moreArticlesNextCtrl,
        scope.moreArticlesSlideContainer,
        () => {
            main(scope)
                // This is called in cb() if AJAX is sucessful on first time
            scope.self.classList.add('active');
            updateButton(scope.moreArticlesPrevCtrl, 'disabled', 'true')
            window.dispatchEvent(customEvent)
        }
    )

}
init(document.querySelector('.more-articles-placeholder'), csn_editorial.moreArticles)