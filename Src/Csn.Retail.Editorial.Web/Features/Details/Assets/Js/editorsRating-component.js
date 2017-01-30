// Details Page css files

require('../css/Modules/Widgets/_editorsRatings.scss');

import Modernizr from 'modernizr';

if (!Modernizr.progressbar, meter) {
    // supported
    // Meter polyfill
    require.ensure(['../../../shared/assets/js/modules/meter/meter.js'],
        function() {
            require('../../../shared/assets/js/modules/meter/meter.js');
        });
}




