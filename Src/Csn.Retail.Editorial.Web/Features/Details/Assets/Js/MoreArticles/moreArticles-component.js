import {lory} from 'lory.js';
import {get} from 'Js/Modules/Ajax/ajax.js'
import moreArticlesContentView from 'Js/MoreArticles/moreArticlesContentView.js'

const classNameSlideContainer = 'lory-slider__slides'
const classNameFrame = 'lory-slider__frame'
const classNamePrevCtrl = 'more-articles__nav-button--prev'
const classNameNextCtrl = 'more-articles__nav-button--next'
const classNameSlide = 'lory-slider__slide'

const scopeSelector = '.more-articles'
const slideContainer = '.' + classNameSlideContainer
const frame = '.' + classNameFrame
const prevCtrl = '.' + classNamePrevCtrl
const nextCtrl = '.' + classNameNextCtrl
const slide = '.' + classNameSlide
const navButtons = '.more-articles__nav-button'
const showHideButton = '.more-articles__button--show-hide'

// Init More Articles Slider
let initMoreArticlesSlider = (selector, options) => {
    const slider = document.querySelector(selector);
    options = Object.assign({}, options);
    return lory(slider, options);
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
    if (scope.querySelector(selector).classList.contains(className)) {
        removeClass(scope, selector, className, text[0][0])
    } else {
        addClass(scope, selector, className, text[0][1])
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
    scope.querySelector(selector).insertAdjacentHTML('beforeend', moreArticlesContentView(data))
}

// Update Content
let updateContent = function(scope, selector, ajax, container, cb) {

    const el = scope.querySelector(selector)
    const query = el ? el.getAttribute('data-more-articles-query'): null;
    const url = el ? el.getAttribute('data-more-articles-path') + query: null;
    let lock = !!scope.querySelector(nextCtrl).getAttribute('data-disabled')
    console.log('lock', lock)

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

    if(!el.classList.contains('active')) {
        //get url and set it to next
        updateButton(scope, nextCtrl, 'data-more-articles-query', el.pathname)
        //destory old slider
        scope.querySelector('.lory-slider__slides').innerHTML = '';
        //Init new slider
        updateContent(
            scope,
            nextCtrl,
            get,
            '.lory-slider__slides',
            () => {
                slider.slideTo(0)
                updateButton(scope, prevCtrl, 'disabled', 'true')
                slider.setup();
            }
        )
    }
}


// handlers
function buttonHandler(scope, slider, firstSlide, visibleSlides) {
    // Prev logic
    if (slider.returnIndex() <= firstSlide) {
        updateButton(scope, prevCtrl, 'disabled', 'true')
    } else {
        updateButton(scope, prevCtrl, 'disabled')
    }

    //Next logic
    if (slider.returnIndex() + visibleSlides >= slidesLength(scope, slide)) {
        updateButton(scope, nextCtrl, 'disabled', 'true')
    } else {
        updateButton(scope, nextCtrl, 'disabled')
    }
}

function nextButtonHandler(scope, slider, offset, cb) {
    // Get slider index and check if offset from end
    if (slider.returnIndex() >= slidesLength(scope, slide) - offset) {
        cb()
    }
}

function buttonShowHideHandler() {
    toggleClass(document, scopeSelector, 'show', ['Show', 'Hide'])
}

function filterShowHideHandler(scope) {
    if (!scope.classList.contains('show')) {
        toggleClass(document, scopeSelector, 'show', ['Show', 'Hide'])
    }
}

// Filters
let filters = (scope, selector, cb, cbArgs) => {
    addEventListenerToButton(scope, selector, 'click', cb, cbArgs)
}


// Main
let main = (scope) => {

    // Init Slider

    const firstSlide = 0
    const visibleSlides = 3;
    const offset = 2 + visibleSlides

    let options = {
        classNameSlideContainer: classNameSlideContainer,
        classNameFrame: classNameFrame,
        classNamePrevCtrl: classNamePrevCtrl,
        classNameNextCtrl: classNameNextCtrl,
        infinite: false,
        rewind: false,
        rewindOnResize: false
    }
    const slider = initMoreArticlesSlider(scopeSelector, options);

    const content = () => { return updateContent(scope, nextCtrl, get, '.lory-slider__slides', () => {
        slider.setup()
    })}

    // Init
    updateButton(scope, prevCtrl, 'disabled', 'true')

    // Init Button Handlers
    addEventListenerToButton(scope, navButtons, 'click', buttonHandler.bind(null, scope, slider, firstSlide, visibleSlides))

    // Init next Button
    addEventListenerToButton(scope, nextCtrl, 'click', nextButtonHandler.bind(null, scope, slider, offset, content))

    //Init Filters
    filters(scope, '.more-articles__filter', filterHandler, [slider, scope])

    // Init show/hide Button
    addEventListenerToButton(scope, showHideButton, 'click', buttonShowHideHandler)

    // Init show hide for filter Button
    addEventListenerToButton(scope, '.more-articles__filter', 'click', filterShowHideHandler.bind(null, scope))

    //Once content is loaded
    if (!scope.classList.contains('show')) {
        toggleClass(document, scopeSelector, 'show', ['Show', 'Hide'])
    }

}

// Call this first to make sure there is content for the slider or it will bug out
updateContent(
    document.querySelector(scopeSelector),
    nextCtrl,
    get,
    '.lory-slider__slides',
    main.bind(null, document.querySelector(scopeSelector))
    )



