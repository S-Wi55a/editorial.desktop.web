import { proxy } from 'Js/Modules/Endpoints/endpoints';

const container = (data) => {

    const filtersLength = data.moreArticleItems.length;
    const moreArticlesPath = proxy;


    const filterView = (str, filter, index) => {
        if (index === 0) {
            return `<a href="${filter.uri || ''}" class="more-articles__filter more-articles__filter--active" data-webm-clickvalue="view-all-articles">${filter.title || ''}</a>`
        } else if (index === filtersLength - 1) {
            return `<a href="${filter.uri || ''}" class="more-articles__filter more-articles__filter--last" data-webm-clickvalue="view-all-articles">${filter.title}</a>`
        } else {
            return `<a href="${filter.uri || ''}" class="more-articles__filter" data-webm-clickvalue="view-all-articles">${filter.title || ''}</a>`
        }
    } 

    return `
        <div class="more-articles" data-webm-section="more-articles">
            <div class="container">
                <div class="more-articles__filters">
                    ${data.moreArticleItems.map((filter, index) => filterView`${filter}${index}`).join('')}
                </div>
                <div class="more-articles__frame swiper-container">
                    <div class="more-articles__slides swiper-wrapper">
                    </div>
                </div>

                <div class="more-articles__nav">
                    <button class="more-articles__button more-articles__button--show-hide" data-webm-clickvalue="hide">${data.headings.showHeading || 'Show'}</button>
                    <button class="more-articles__nav-button more-articles__nav-button--prev" data-webm-clickvalue="previous">Prev</button>
                    <button class="more-articles__nav-button more-articles__nav-button--next" data-webm-clickvalue="next"
                        data-more-articles-path="${moreArticlesPath || ''}" 
                        data-more-articles-query="${data.moreArticleItems[0].uri || ''}">Next</button>
                </div>
            </div>
        </div>
        `
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