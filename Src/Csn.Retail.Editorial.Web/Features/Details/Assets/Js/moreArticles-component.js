require('Css/Modules/Lory/_lory.scss');

import {lory} from 'lory.js';
import { contentLoaded } from 'document-promises';



// Init More Articles Slider
let initMoreArticlesSlider = (selector) => {
    const slider = document.querySelector(selector);

    lory(slider, {
        // options going here
    });
}

// Toggle hide/show more Articles


// Previous button - with logic if at start

// Next button - w/ logic for lazy loading and if reached end


contentLoaded.then(() => {
    /* document is ready */
});