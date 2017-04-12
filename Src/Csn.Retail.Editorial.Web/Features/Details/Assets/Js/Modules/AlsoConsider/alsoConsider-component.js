﻿import 'Css/Modules/Widgets/AlsoConsider/_alsoConsider.scss'

import * as View from 'Js/Modules/AlsoConsider/alsoConsider-view.js'
import * as Ajax from 'Js/Modules/Ajax/ajax.js'

// Get Scope and cache selectors
const scope = document.querySelector('.also-consider-placeholder');

// Make Query - Ajax
const makeQuery = (url, el, view, cb = () => { }, onError = () => { }) => {

    //Make Query
    Ajax.get(url,
        (resp) => {
            //update list
            el.innerHTML = view(JSON.parse(resp))
            cb()
        },
        () => {
            onError()
        }
    )
}

// Init
const init = (scope, data) => {
    //Render container
    scope.innerHTML = View.container(data)

    // Setup vars
    const alsoConsider = scope.querySelector('.also-consider');

    // Load data
    alsoConsider.classList.toggle('loading');
    makeQuery(
        alsoConsider.getAttribute('data-also-consider-query'),
        alsoConsider,
        View.inner,
        () => {
            alsoConsider.classList.toggle('loading');
        }
    )
}

init(scope, csn_editorial.alsoConsider)