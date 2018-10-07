import { proxyEndpoint } from 'Endpoints/endpoints';

const container = (data) => {

    const alsoConsiderQueryPath = proxyEndpoint;

    return `<div class="also-consider" data-also-consider-query="${alsoConsiderQueryPath}${data.uri}"></div>`
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
                                <img class="salso-consider-item__image" src="${item.image.url}?width=167"
                                    srcset="${item.image.url}?width=167 167w"
                                    sizes="(min-width: 768px) 167px" alt="${item.image.alternateText || ''}" />
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
