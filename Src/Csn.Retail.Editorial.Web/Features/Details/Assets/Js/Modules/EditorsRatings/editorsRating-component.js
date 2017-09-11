// Details Page css files
require('Css/Modules/Widgets/EditorRatings/_editorsRatings.scss');

import Modernizr from 'modernizr';
import Circles from 'circles';
import TinyAnimate from 'TinyAnimate';
import ScrollMagic from 'ScrollMagic';

const editorsRatings = '.editors-ratings';
const expertRatingMeters = document.querySelectorAll('.expert-ratings__meter');

const overallRatingGraph = {
    id: 'overall-rating-graph',
    radius: 60,
    value: 0,
    maxValue: 100,
    width: 10,
    text: function(value) { return value; },
    colors: ['#DBDBDB', '#007CC2'],
    duration: 0,
    wrpClass: 'overall-rating-graph__wrp',
    textClass: 'overall-rating-graph__text',
    valueStrokeClass: 'overall-rating-graph__valueStroke',
    maxValueStrokeClass: 'overall-rating-graph__maxValueStroke',
    styleWrapper: true,
    styleText: true
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


let init = (d, w, scope) => {

    const myCircle = Circles.create(overallRatingGraph);
    myCircle.update(-1, 0);

    // Set scene
    const triggerElement = scope
    const triggerHook = 0.6
    w.scrollMogicController = w.scrollMogicController || new ScrollMagic.Controller();


    if (!Modernizr.meter) {
        // Meter polyfill
        require.ensure(['Js/Modules/Meter/meter.js'],
            function() {
                for (var meter of expertRatingMeters) {
                    meter.setAttribute('value', meter.getAttribute('data-value'));
                }
                require('Js/Modules/Meter/meter.js');

                new ScrollMagic.Scene({
                        triggerElement: triggerElement,
                        triggerHook: triggerHook,
                        reverse: false
                    })
                    .on("enter", () => {
                        myCircle.update(d.getElementById('overall-rating-graph').getAttribute('data-overall-rating'), 500)
                    })
                    .addTo(window.scrollMogicController);
            },
            'meter-polyfill');
    } else {
        new ScrollMagic.Scene({
                triggerElement: triggerElement,
                triggerHook: triggerHook,
                reverse: false
            })
            .on("enter", () => {
                myCircle.update(d.getElementById('overall-rating-graph').getAttribute('data-overall-rating'), 500)
                AnimateMeters(expertRatingMeters, 200);
            })
            .addTo(window.scrollMogicController);
    }

}
init(document, window, editorsRatings);