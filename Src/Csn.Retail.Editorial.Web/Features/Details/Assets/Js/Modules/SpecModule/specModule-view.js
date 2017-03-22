const container = (data) => {

    if (!Array.isArray(data)) { return false }

    const moreArticlesPath = "/editorial/api/v1/spec/?uri=";

    return `
        <div class="spec-module">
            <div class="slideshow slideshow--spec-module">
                <div class="slideshow__container swiper-container">

                    <div class="slideshow__slides swiper-wrapper">
                        ${data.map(item => `
                            <div class="slideshow__slide swiper-slide" data-spec-url="${moreArticlesPath}${item.uri}"></div>
                        `).join('')}   
                    </div>

                    <div class="slideshow__pagination-label slideshow__pagination-label--min"></div>
                    <div class="slideshow__pagination swiper-pagination"></div>
                    <div class="slideshow__pagination-label slideshow__pagination-label--max"></div>

                </div>
            </div>
        </div>
     `
}


const item = (data) => {


    const specifications = (str, items) => {
        return `
            <dl class="spec-item__spec-item-list">
                ${items.map(item => `
                    <dt class="spec-item__spec-item-title">${item.title}</dt>
                    <dd class="spec-item__spec-item-value" data-value="${item.value}">${item.value}</dd>
                `).join('')} 
            </dl>
        `
    } 

    const stratton = (str, data) => {
        return `
                <div class="spec-item__third-party-offer spec-item__third-party-offer--stratton third-party-offer">
                    <img class= "third-party-offer__logo" src="${data.logoUrl}" />
                    <h3 class="third-party-offer__heading">${data.headings.title}</h3>
                    <div class="third-party-offer__price">${data.monthlyRepayments}
                        <span class="third-party-offer__price-term" data-disclaimer="${data.disclaimer}">
                            ${data.headings.frequentPayment}
                        </span>
                    </div>
                    <a href="${data.formUrl}" class="third-party-offer__link">${data.headings.getQuote}</a>
                </div>
        `
    } 

    const budgetDirect = (str, data) => {
        return `
                <div class="spec-item__third-party-offer spec-item__third-party-offer--stratton third-party-offer">
                    <img class= "third-party-offer__logo" src="${data.logoUrl}" />
                    <h3 class="third-party-offer__heading">${data.headings.title}</h3>
                    <div class="third-party-offer__price">${data.monthlyRepayments}
                        <span class="third-party-offer__price-term" data-disclaimer="${data.disclaimer}">
                            ${data.headings.frequentPayment}
                        </span>
                    </div>
                </div>
        `
    } 

    return `
        <div class="spec-item">
            <h2 class="spec-item__heading">${data.title}</h2>
            <p class="spec-item__subheading">${data.description}</p>
            <div class="spec-item__image-container">
                <img class="spec-item__image" src="${data.image.url}" alt="${data.image.alternateText}"/>
            </div>
            ${specifications`${data.items}`}
            <div class="spec-item__third-party-offers">
                ${stratton`${data.strattonData}`}
                ${budgetDirect`${data.budgetDirectData}`}
            </div>

        </div>
    `

}


export { container, item };