import Modal from 'Js/Modules/Modal/modal.js'
import {disclaimerTemplate} from 'Js/Modules/Modal/modal-disclaimer-view'

// display disclaimer on pricing guide
window.csn_modal = window.csn_modal || new Modal();

let pricingLabel = document.querySelector('.article__pricing-label');

if (pricingLabel) {
    pricingLabel.addEventListener('click',
        (e) => {
            window.csn_modal.show(disclaimerTemplate(e.target.getAttribute('data-disclaimer'), 'pricing-guide-disclaimer'));
        });
}