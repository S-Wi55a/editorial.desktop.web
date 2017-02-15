// Details Page css files

require('css/Modules/Widgets/_editorsRatings.scss');

import Modernizr from 'modernizr';
import Circles from 'circles';
import TinyAnimate from 'TinyAnimate';

const expertRatingSelector = '.expert-ratings';
const expertRatingMeters = document.querySelectorAll('.expert-ratings__meter');

const overallRatingGraph = {
    id:                  'overall-rating-graph',
    radius:              60,
    value:               document.getElementById('overall-rating-graph').getAttribute('data-overall-rating'),
    maxValue:            100,
    width:               10,
    text:                function(value){return value;},
    colors:              ['#DBDBDB', '#007CC2'],
    duration:            500,
    wrpClass:            'overall-rating-graph__wrp',
    textClass:           'overall-rating-graph__text',
    valueStrokeClass:    'overall-rating-graph__valueStroke',
    maxValueStrokeClass: 'overall-rating-graph__maxValueStroke',
    styleWrapper:        true,
    styleText:           true
};


let AnimateMeters = function(listOfMeter, timeBetween = 400, duration) {

    const LIST = listOfMeter;
    const LENGTH = LIST.length;
    const DELAY = timeBetween;
    const DURRATION = duration;

    const w = window;

    for (let i = 0; i < LENGTH; i++) {
        w.setTimeout(AnimateMeter.bind(undefined, i, LIST, DURRATION), DELAY * i)
    }


};

let AnimateMeter = function(index, list, duration = 1000) {

    const VALUE = parseInt(list[index].getAttribute('data-value'));

    TinyAnimate.animate(0,
        VALUE,
        duration,
        function(x) {
            list[index].setAttribute('value', x);
        },
        'easeOutCubic');
};

function isInViewport(selector, threshold = 1, cb) {
    const sections = document.querySelectorAll(selector);

    window.addEventListener('scroll', inView)
    inView();

    function inView() {
        // Don't run the rest of the code if every section is already visible
        if (document.querySelectorAll(selector + ':not(.visible)').length === 0) return;

        // Run this code for every section in sections
        for (const section of sections) {
            if (section.getBoundingClientRect().top <= window.innerHeight * threshold && section.getBoundingClientRect().top > 0) {
                section.classList.add('visible');
                cb()
                window.removeEventListener('scroll', inView)
            }
        }
    }
}

if (!Modernizr.meter) {
    // Meter polyfill
    require.ensure(['Js/Modules/meter/meter.js'],
        function() {
            for (var meter of expertRatingMeters) {
                meter.setAttribute('value', meter.getAttribute('data-value'));
            }
            require('Js/Modules/meter/meter.js');
            isInViewport(expertRatingSelector, 0.75, () => {
                Circles.create(overallRatingGraph);
            });
        },
        'meter-polyfill');
} else {
    isInViewport(expertRatingSelector, 0.75, () => {
        Circles.create(overallRatingGraph);
        AnimateMeters(expertRatingMeters, 200);
    });
}