import 'Css/Modules/Widgets/StockForSale/_stockForSale.scss'

import * as view from 'Js/Modules/StockForSale/stockForSale-view.js'
import * as ajax from 'Js/Modules/Ajax/ajax.js'


const scope = document.querySelector('.stock-for-sale-placeholder')
const stockForSaleOption = '.stock-for-sale-options__option'
const stockForSaleList = '.stock-for-sale__list'
const stockForSaleSelect = '.stock-for-sale__select'


// load list of items
const getAttrValue = (scope, selector, attr) => {

    return scope.querySelector(selector).getAttribute(attr)
  
}

// Set Button
const setAttrValue = (scope, selector, attr, val) => {
    scope.querySelector(selector).setAttribute(attr, value)
}

// Make Query - Ajax
const makeQuery = (url, scope, selector) => {

    //Make Query
    ajax.get(url, (resp) => {
        //update list
        scope.querySelector(selector).innerHTML = view.listItem(JSON.parse(resp))
    })
}

const toggleCLass = (scope, selector, className) => {

    scope.querySelector(selector).classList.contains(className)
        ? scope.querySelector(selector).classList.remove(className)
        : scope.querySelector(selector).classList.add(className);
}

// Init
const init = (scope, data) => {
    //Render container
    scope.innerHTML = view.container(data)

    makeQuery(getAttrValue(scope, stockForSaleOption, 'data-stock-for-sale-query'), scope, stockForSaleList)

    scope.querySelector(stockForSaleSelect).addEventListener('click', toggleCLass.bind(scope, stockForSaleOption, 'show')
}

init(scope, csn_editorial.stockForSale)