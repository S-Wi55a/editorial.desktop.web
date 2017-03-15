const container = (data) => {

    const moreArticlesPath = "/editorial/api/v1/stock-listing/?uri=";
    const limit = '%26limit=2';

    return (`
        <div class="stock-for-sale">
            <h2 class="stock-for-sale__header">${data.Heading}</h2>
            <button class="stock-for-sale__select"></button>
            <ul class="stock-for-sale__list"></ul>
            <div class="stock-for-sale__button-container">
                <a class="stock-for-sale__button stock-for-sale__button--view-all" href="">
                    ${data.ViewAllStockButton}
                </a>
            </div>
            <div class="stock-for-sale-options">
                <ul class="stock-for-sale-options__list">
                    ${data.Filters.map(filter => `
                        <li
                            class="stock-for-sale-options__option"
                            data-stock-for-sale-query="${moreArticlesPath}${filter.Query}${limit}" 
                            data-stock-for-sale-view-all-url="${filter.ViewAllUrl}">
                            ${filter.Name}
                        </li>
                    `).join('')}
                </ul>
            </div>
        </div>
        `)
}

const listItem = (data) => {
    const template = data['Items'].map((item) => {
        return (`
            <li class="stock-for-sale-item">
                <a href="${item.DetailsPageUrl}">
                    <img class="stock-for-sale-item__image" src=${item.PhotoUrl}" />
                </a>
                <a href="${item.DetailsPageUrl}">
                    <h3 class="stock-for-sale-item__title">${item.Title}</h3>
                </a>
                <p class="stock-for-sale-item__price">${item.Price}</p>
                <ul class="stock-for-sale-item__list">
                    ${item.Attributes.map(attr => `
                        <li class="stock-for-sale-item__list-item">${attr}</li>
                    `).join('')}
                </ul>
                <p class="stock-for-sale-item__location">${item.Location}</p>
            </li>
            `)
    })

    return template.reduce((prev, current) => {
        return prev + current
    })
}



export { container, listItem };