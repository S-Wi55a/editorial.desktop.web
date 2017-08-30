import 'Css/Modules/Widgets/StockForSale/_stockForSale.scss'

import * as View from 'Js/Modules/StockForSale/stockForSale-view'
import * as Ajax from 'Js/Modules/Ajax/ajax.js'

import Modal from 'Js/Modules/Modal/modal.js'
import { disclaimerTemplate } from 'Js/Modules/Modal/modal-disclaimer-view'
 

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

            resp = JSON.parse(resp);
            //update list
            el.innerHTML = View.listItem(resp);
            cb(resp);
        },
        () => {
            onError();
        }
    );
}

window.csn_modal = window.csn_modal || new Modal();

// add click handlers for displaying pricing disclaimers for each stock item
const initPriceDisclaimers = () => {
    Array.from(scope.querySelectorAll('.stock-for-sale-item-pricing__label')).forEach((el) => {
        el.addEventListener('click',
            (e) => {
                window.csn_modal.show(disclaimerTemplate(e.target.getAttribute('data-disclaimer'), 'pricing-guide-disclaimer'));
            }
        );
    });
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
    const stockForSaleOption = scope.querySelectorAll('.stock-for-sale-options__option');
    const stockForSaleList = scope.querySelector('.stock-for-sale__list');
    const stockForSaleSelect = scope.querySelector('.stock-for-sale__select');
    const stockForSaleButton = scope.querySelector('.stock-for-sale__button');
    

    // Load data
    toggleCLass(stockForSale, 'loading')
    makeQuery(
        getAttrValue(stockForSaleOption[0], 'data-stock-for-sale-query'),
        stockForSaleList,
        (data) => {
            //Hide modue if no items in all states
            if (data && data.items.length > 0) {
                stockForSale.classList.add('active');
                initPriceDisclaimers();
            }

            toggleCLass(stockForSale, 'loading');
            setAttrValue(stockForSaleButton, 'href', getAttrValue(stockForSaleOption[0], 'data-stock-for-sale-view-all-url'));
        },
        // On Error
        () => {
            toggleCLass(stockForSale, 'loading')  
        }
    )

    stockForSaleSelect.addEventListener('click',
        () => {
            toggleCLass(stockForSale, 'show--nav');
            animateNavItems(stockForSaleOption, 'easeOutBack', 100);
        }
    );

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