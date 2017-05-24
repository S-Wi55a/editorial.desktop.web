import { proxy } from 'Js/Modules/Endpoints/endpoints';

const container = (data) => {

    const alsoConsiderQueryPath = proxy;

    return `<div class="also-consider" data-webm-section="also-consider" data-also-consider-query="${alsoConsiderQueryPath}${data.uri}"></div>`
}

const inner = (data) => {

    const key = 'alsoConsiderItems';

    return `
            <h2 class="also-consider__header">${data.heading || ''}</h2>
            <ul class="also-consider__list">
                ${data[key].map((item) => {
                    return `
                        <li class="also-consider-item">
                            <a href="${item.articleUrl || ''}" data-webm-clickvalue="click-post">
                                <img class="also-consider-item__image" src="${item.image.url || ''}" alt="${item.image.alternateText || ''}"/>
                                <h3 class="also-consider-item__title">${item.linkText || ''}</h3>
                                <p class="also-consider-item__description">${item.description || ''}</p>
                            </a>
                        </li>
                        `
                }).join('')}
            </ul>
            `
}



export { container, inner };
