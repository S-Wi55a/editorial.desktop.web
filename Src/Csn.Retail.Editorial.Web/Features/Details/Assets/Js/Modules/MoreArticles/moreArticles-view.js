import { proxy } from 'Js/Modules/Endpoints/endpoints';

const container = (data) => {

    const moreArticlesPath = proxy;

    const filterView = (str, filter) => {
            return `<div class="more-articles__filter more-articles__filter--active" data-webm-clickvalue="view-${encodeURI(filter.title.toLowerCase())}">${filter.title}</div>`;
    }

    const linkView = (str, link) => {
        return `<a href="${link.uri}" class="more-articles__link more-articles__link--last" data-webm-clickvalue="view-${encodeURI(link.text.toLowerCase())}">${link.text}</a>`;
    }

    return `
        <div class="more-articles" data-webm-section="more-articles">
            <div class="container">
                <div class="more-articles__top-container">
                    <div class="more-articles__filters">
                        ${data.filters.map((filter, index) => filterView`${filter}${index}`).join('')}
                    </div>
                    <div class="more-articles__links">
                        ${data.links.map((link) => linkView`${link}`).join('')}
                    </div>
                </div>
                <div class="more-articles__frame swiper-container">
                    <div class="more-articles__slides swiper-wrapper">
                    </div>
                </div>
                <div class="more-articles__nav">
                    <button class="more-articles__button more-articles__button--show-hide" data-webm-clickvalue="hide">${data.showText}</button>
                    <button class="more-articles__nav-button more-articles__nav-button--prev" data-webm-clickvalue="previous">Prev</button>
                    <button class="more-articles__nav-button more-articles__nav-button--next" data-webm-clickvalue="next"
                        data-more-articles-path="${moreArticlesPath || ''}" 
                        data-more-articles-query="${data.filters[0].uri || ''}">Next</button>
                </div>
            </div>
        </div>
        `;
}


const article = (data) => {


    const key = 'items';

    if (!Array.isArray(data[key])) { return '' }


    const template = data[key].map((item) => {
        return (`
            <div class="more-articles__slide swiper-slide">
                <div class="more-article">
                    <a class="more-article__link-container" data-webm-clickvalue="click-post" href="${item.url || ''}">
                        <div class="more-article__image">
                            <img src="${item.image.url || ''}?width=140&height=93" alt="${item.image.alternateText || ''}" />
                        </div>
                        <div class="more-article__content">
                            <div class="more-article__title">
                                <h2 >${item.headline || ''}</h2>
                            </div>
                            <p class="more-article__link">${data.readMore || ''}</p>
                            ${item.sponsored ?
                                `<div class="more-article__banner more-article__banner--${item.sponsored || ''}">${item.sponsored || ''}</div>`
                            : ''}
                        </div>
                    </a>
                </div>
            </div>
        `)
    })

    if (data[key].length) {

        return template.reduce((prev, current) => {
            return prev + current
        })

    } else {

        return template
    }

}


export { container, article };