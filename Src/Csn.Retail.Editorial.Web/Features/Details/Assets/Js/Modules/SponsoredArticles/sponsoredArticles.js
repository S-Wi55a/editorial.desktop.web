import Modal from 'Js/Modules/Modal/modal.js'

import 'Css/Modules/SponsoredArticles/_sponsoredArticles.scss';

window.csn_modal = window.csn_modal || new Modal()

const description = (data) => {

    return `
        <div class="sponsored-description">
            <div class="sponsored-description__content">
                <p>${data || ''}</p>
            </div>
        </div>
    `
}

document.querySelector('.article__type--sponsored').addEventListener('click', (e) => {
    const content = decodeURI(e.target.getAttribute('data-description'))
    window.csn_modal.show(description(content), 'sponsored-article')
});