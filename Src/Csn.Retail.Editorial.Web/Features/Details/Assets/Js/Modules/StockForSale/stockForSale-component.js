import 'Css/Modules/Widgets/StockForSale/_stockForSale.scss'

import * as View from 'Js/Modules/StockForSale/stockForSale-view.js'
import * as Ajax from 'Js/Modules/Ajax/ajax.js'
 

// Get Scope and cache selectors
const scope = document.querySelector('.stock-for-sale-placeholder');

// Get Attr
const getAttrValue = (el, attr) => {
    return el.getAttribute(attr)
  
}

// Set Attr
const setAttrValue = (el, attr, val) => {
    el.setAttribute(attr, val)
}


// Make Query - Ajax
const makeQuery = (url, el, cb = () => {}, onError = () => {}) => {

    //Make Query
    Ajax.get(url,
        (resp) => {
            //update list
            el.innerHTML = View.listItem(JSON.parse(resp))
            cb()
        },
        () => {
            onError()  
        }
    )
}

const toggleCLass = (el, className) => {

    el.classList.contains(className) ? el.classList.remove(className) : el.classList.add(className);

}


const animateNavItems = (elList, className, timeBetween = 400) => {

    const LENGTH = elList.length;

    for (let i = 0; i < LENGTH; i++) {
        window.setTimeout(toggleCLass.bind(null, elList[i], className), timeBetween * i)
    }

}

// Init
const init = (scope, data) => {
    //Render container
    scope.innerHTML = View.container(data)

    // Setup vars
    const stockForSale = scope.querySelector('.stock-for-sale');
    const stockForSaleOptions = scope.querySelector('.stock-for-sale-options');
    const stockForSaleOption = scope.querySelectorAll('.stock-for-sale-options__option');
    const stockForSaleList = scope.querySelector('.stock-for-sale__list');
    const stockForSaleSelect = scope.querySelector('.stock-for-sale__select');
    const stockForSaleButton = scope.querySelector('.stock-for-sale__button')

    // Load data
    toggleCLass(stockForSale, 'loading')
    makeQuery(
        getAttrValue(stockForSaleOption[0], 'data-stock-for-sale-query'),
        stockForSaleList,
        () => {
            stockForSale.classList.add('active')
            toggleCLass(stockForSale, 'loading')
            setAttrValue(stockForSaleButton, 'href', getAttrValue(stockForSaleOption[0], 'data-stock-for-sale-view-all-url'))
        },
        // On Error
        () => {
            toggleCLass(stockForSale, 'loading')  
        }
    )

    stockForSaleSelect.addEventListener('click',
        () => {
            toggleCLass(stockForSale, 'show--nav')
            animateNavItems(stockForSaleOption, 'easeOutBack', 100)
        }
    )

    Array.from(stockForSaleOption).forEach((el) => {
        el.addEventListener('click', () => {
            toggleCLass(stockForSale, 'loading')
            makeQuery(
                getAttrValue(el, 'data-stock-for-sale-query'),
                stockForSaleList,
                () => {
                    toggleCLass(stockForSale, 'show--nav')
                    toggleCLass(stockForSale, 'loading')
                    animateNavItems(stockForSaleOption, 'easeOutBack', 100)
                    stockForSaleSelect.innerHTML = el.innerHTML
                    setAttrValue(stockForSaleButton, 'href', getAttrValue(el, 'data-stock-for-sale-view-all-url'))
                },
                // On Error
                () => {
                    toggleCLass(stockForSale, 'loading')
                }
            )
        })
    })
}

init(scope, csn_editorial.stockForSale)