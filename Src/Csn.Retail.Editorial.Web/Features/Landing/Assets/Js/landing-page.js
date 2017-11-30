require('Css/landing-page.scss');

import { configureStore } from 'Redux/Global/Store/store.client.js'
if (process.env.DEBUG) { require('debug.addIndicators'); }

//Enable Redux store globally
window.store = configureStore(); //Init store

// load Redux 
(function redux(d) {

    // Check if there is a preloaded state
    const initState = window.__PRELOADED_STATE__store.landing

    window.store.addReducer('store', require('carousel/Reducers').carouselParentReducer(initState));
        
    if (d.querySelector('.csn-carousel')) {
        const carousels = [...document.querySelectorAll('.csn-carousel')]
        carousels.forEach((el) => require('carousel/carousel').default(el))
    }

})(document);
