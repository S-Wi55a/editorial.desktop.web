// Details Page css files

require('../css/Modules/Widgets/_editorsRatings.scss');

import Modernizr from 'modernizr';
import Circles from 'circles';
import TinyAnimate from 'TinyAnimate'

if (!Modernizr.meter) {
    // Meter polyfill
    require.ensure(['../../../shared/assets/js/modules/meter/meter.js'],
        function() {
            require('../../../shared/assets/js/modules/meter/meter.js');
        }, 'meter-polyfill');
}



var myCircle = Circles.create({
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
});


let AnimateMeters = function(listOfMeter, timeBetween = 400) {

    const LIST = listOfMeter;
    const LENGTH = LIST.length;
    const DELAY = timeBetween;
    const w = window;

    for (let i = 0; i < LENGTH; i++) {
        w.setTimeout(AnimateMeter.bind(undefined,i,LIST), DELAY*i)
    }

}

let AnimateMeter = function(index, list, duration = 1000) {

    const VALUE = parseInt(list[index].getAttribute('data-value'));

    TinyAnimate.animate(0, VALUE, duration, function(x) {
        list[index].setAttribute('value', x);
    }, 'easeOutCubic');
}

AnimateMeters(document.querySelectorAll('.expert-ratings__meter'), 200);


