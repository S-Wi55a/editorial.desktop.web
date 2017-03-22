const container = (data) => {

    const stockForSalePath = "/editorial/api/v1/stock-listing/?uri=";
    const limit = '%26limit=2';

    return `
        <div class="stock-for-sale">
            <h2 class="stock-for-sale__header">${data.heading}</h2>
            <button class="stock-for-sale__select">${data.filters[0].name}</button>
            <ul class="stock-for-sale__list"></ul>
            <div class="stock-for-sale__button-container">
                <a class="stock-for-sale__button stock-for-sale__button--view-all" href="">
                    ${data.viewAllStockButton}
                </a>
            </div>
            <div class="stock-for-sale-options">
                <ul class="stock-for-sale-options__list">
                    ${data.filters.map(filter => `
                        <li
                            class="stock-for-sale-options__option"
                            data-stock-for-sale-query="${stockForSalePath}${filter.query}${limit}" 
                            data-stock-for-sale-view-all-url="${filter.viewAllUrl}">
                            ${filter.name}
                        </li>
                    `).join('')}
                </ul>
            </div>
        </div>
        `
}

const listItem = (data) => {

    const key = 'items';

    if (data[key].length) {
        const template = data[key].map((item) => {
            return (`
                <li class="stock-for-sale-item">
                    <a href="${item.detailsPageUrl}">
                        <img class="stock-for-sale-item__image" src="${item.photoUrl}" />
                    </a>
                    <a href="${item.detailsPageUrl}">
                        <h3 class="stock-for-sale-item__title">${item.title}</h3>
                    </a>
                    <p class="stock-for-sale-item__price">${item.price}</p>
                    <ul class="stock-for-sale-item__list">
                        ${item.attributes.map(attr => `
                            <li class="stock-for-sale-item__list-item">${attr}</li>
                        `).join('')}
                    </ul>
                    <p class="stock-for-sale-item__location">${item.location}</p>
                </li>
                `)
        })

        return template.reduce((prev, current) => {
            return prev + current
        })

    } else {

        return `<li class="stock-for-sale-item stock-for-sale-item--no-items">${data.responseMessage}</li>`
    }


}



export { container, listItem };
