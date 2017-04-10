// Container
const container = (data) => {

    const specPath = "/editorial/api/v1/spec/?uri=";

    return `
        <div class="spec-module">
            <div class="slideshow slideshow--spec-module">
                <div class="slideshow__container swiper-container">

                    <div class="slideshow__slides swiper-wrapper">
                        ${data.items.map(item => `
                            <div class="slideshow__slide swiper-slide" data-spec-url="${specPath || '' }${item.uri || '' }"></div>
                        `).join('')}   
                    </div>
                    <div class="slideshow__nav-wrapper">
                        <div class="slideshow__pagination-label slideshow__pagination-label--min">${data.minLabel || ''}</div>
                        <div class="slideshow__pagination-wrapper">                        
                            <div class="slideshow__pagination swiper-pagination"></div>
                        </div>
                        <div class="slideshow__pagination-label slideshow__pagination-label--max">${data.maxLabel || ''}</div>
                    </div>
                </div>
            </div>
        </div>
     `
}

// Items - Slide content
const item = (data) => {

    const specsLength = data.items.length || 0

    const specificationsItem = (str, item, index) => {
        if (index === specsLength - 1) {
            return `
                    <dt class="spec-item__spec-item-title">${item.title || ''}</dt>
                    <dd class="spec-item__spec-item-value" data-value="${item.value || ''}">${item.value || ''}</dd>
            `
        } else {
            return `
                    <dt class="spec-item__spec-item-title">${item.title || ''}</dt>
                    <dd class="spec-item__spec-item-value">${item.value || ''}</dd>
            `
        }
    } 

    const specifications = (str, items) => {
        return `
            <h3 class="spec-item__spec-item-list-heading">Specification</h3>
            <dl class="spec-item__spec-item-list">
                ${items.map((item, index) => specificationsItem`${item}${index}`).join('')} 
            </dl>
        `
    } 

    const stratton = (str, data) => {
        if(!data) { return ''}
        return `
                <div class="spec-item__third-party-offer spec-item__third-party-offer--stratton third-party-offer">
                    <img class= "third-party-offer__logo" src="${data.logoUrl || ''}" />
                    <div class="third-party-offer__content">
                        <h3 class="third-party-offer__heading">${data.headings.title || ''}</h3>
                        <div class="third-party-offer__price-container">
                            <span class="third-party-offer__price">
                                ${data.monthlyRepayments || ''}
                            </span>
                            <span class="third-party-offer__price-term" data-disclaimer="'${encodeURI(data.disclaimer) || ''}">
                                ${data.headings.frequentPayment || ''}
                            </span>
                        </div>
                    </div>
                    <a href="${data.formUrl || ''}" class="third-party-offer__link">${data.headings.getQuote || ''}</a>
                </div>
        `
    } 

    const budgetDirect = (str, data) => {
        if (!data) { return '' }
        return `
                <div class="spec-item__third-party-offer spec-item__third-party-offer--stratton third-party-offer">
                    <img class= "third-party-offer__logo" src="${data.logoUrl || ''}" />
                    <div class="third-party-offer__content">
                        <h3 class="third-party-offer__heading">${data.headings.title || ''}</h3>
                        <div class="third-party-offer__price-container">
                            <span class="third-party-offer__price">
                                ${data.annualCost || ''}
                            </span>
                            <span class="third-party-offer__price-term" data-disclaimer="${encodeURI(data.disclaimer) || ''}">
                                ${data.headings.frequentPayment || ''}
                            </span>
                        </div>
                    </div>
                    <a href="${data.formUrl || ''}" class="third-party-offer__link">${data.headings.getQuote || ''}</a>
                    <div class="third-party-offer__terms-and-conditions">${data.termCondition}</div>
                </div>
        `
    } 

    const price = (str, data) => {
        if (data.priceNew) {
            return `
                    <p class="spec-item__price">${data.priceNew.price || ''}</p>
                    <p class="spec-item__price-disclaimer" data-disclaimer="${encodeURI(data.priceNew.disclaimerText) || ''}">${data.priceNew.disclaimerTitle || ''}</p>
            `
        } else {
            return `
                    <div class="spec-item__price-container">
                        <div class="spec-item__price-label">${data.pricePrivate.heading || ''}</div>
                        <div class="spec-item__price spec-item__price--price-private">${data.pricePrivate.priceRange || ''}</div>
                    </div>
                    <div class="spec-item__price-container">
                        <div class="spec-item__price-label">${data.priceTradeIn.heading || ''}</div>
                        <div class="spec-item__price spec-item__price--price-trade-in">${data.priceTradeIn.priceRange || ''}</div>
                    </div>
            `
        }
    } 

    return `
        <div class="spec-item">
            <div class="spec-item__column spec-item__column--1">
                <h2 class="spec-item__heading">${data.title || ''}</h2>
                <p class="spec-item__subheading">${data.description || ''}</p>
                ${price`${data}`}
                <div class="spec-item__image-container">
                    ${data.image ? `<img class="spec-item__image" src="${data.image.url || ''}" alt="${data.image.alternateText || ''}" />` : ``}
                </div>
                <div class="spec-item__third-party-offers">
                    ${stratton`${data.strattonData}`}
                    ${budgetDirect`${data.budgetDirectData}`}
                </div>
            </div>
            <div class="spec-item__column spec-item__column--2">
                ${specifications`${data.items}`}
            </div>
        </div>
    `

}

const disclaimer = (data) => {

    return `
        <div class="spec-module-disclaimer">
            <div class="spec-module-disclaimer__content">
                <p>${data || ''}</p>
            </div>
        </div>
    `
}

export { container, item, disclaimer };