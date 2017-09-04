import { proxy } from 'Js/Modules/Endpoints/endpoints';

const container = (data) => {

    const stockForSalePath = proxy;
    const limit = '%26limit=2';
 
    return `
        <div class="stock-for-sale">
            <h2 class="stock-for-sale__header">${data.heading || ''}</h2>
            <button class="stock-for-sale__select" data-webm-clickvalue="states">${data.filters[0].name || ''}</button>
            <ul class="stock-for-sale__list"></ul>
            <div class="stock-for-sale__button-container">
                <a class="stock-for-sale__button stock-for-sale__button--view-all" href="" data-webm-clickvalue="view-all-stock">
                    ${data.viewAllStockButton || ''}
                </a>
            </div>
            <div class="stock-for-sale-options">
                <ul class="stock-for-sale-options__list">
                    ${data.filters.map(filter => `
                        <li
                            class="stock-for-sale-options__option"
                            data-stock-for-sale-query="${stockForSalePath || ''}${filter.stockQuery || ''}${limit || ''}"
                            data-stock-for-sale-view-all-url="${filter.viewAllUrl || ''}">
                            ${filter.name || ''}
                        </li>
                    `).join('')}
                </ul>
            </div>
        </div>
        `
}

const listItem = (data) => {

    const key = 'items';

    if (data && data[key].length) {
        const template = data[key].map((item) => {
            return (`
                <li class="stock-for-sale-item">
                    <a href="${item.detailsPageUrl || ''}" data-webm-clickvalue="stock-clicked">
                        <img class="stock-for-sale-item__image" src="${item.photoUrl}?width=238"
                            srcset="${item.photoUrl}?width=238 238w"
                            sizes="(min-width: 768px) 238px" />
                    </a>
                    <a href="${item.detailsPageUrl || ''}" data-webm-clickvalue="stock-clicked">
                        <h3 class="stock-for-sale-item__title">${item.title || ''}</h3>
                    </a>
                    ${priceData(item.priceData)}
                    <ul class="stock-for-sale-item__list">
                        ${item.attributes.map(attr => `
                            <li class="stock-for-sale-item__list-item">${attr || ''}</li>
                        `).join('')}
                    </ul>
                    <p class="stock-for-sale-item__location">${item.location || ''}</p>
                </li>
                `);
        });

        return template.reduce((prev, current) => {
            return prev + current;
        });
    } else {
        return `<li class="stock-for-sale-item stock-for-sale-item--no-items">${ (data && data.responseMessage) ? data.responseMessage : ''}</li>`;
    }
};

const priceData = (data) => {
    if (data && data.text) {
        return (`
            <div class="stock-for-sale-item-pricing">
                <p class="stock-for-sale-item-pricing__price">${data.text}</p>
                <p class="stock-for-sale-item-pricing__label" data-disclaimer="${data.disclaimer}">${data.label || ''}</p>
            </div>
        `);
    }

    return (``);
};


export { container, listItem };
