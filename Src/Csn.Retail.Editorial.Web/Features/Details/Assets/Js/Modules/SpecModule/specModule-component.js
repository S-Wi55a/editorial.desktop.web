import Swiper from 'swiper'
import * as Ajax from 'Js/Modules/Ajax/ajax.js'
import * as View from 'Js/Modules/SpecModule/specModule-view.js'

// Get Scope and cache selectors
const scope = document.querySelector('.spec-module-placeholder');

let init = (scope, data) => {

    //Render Container
    scope.innerHTML = View.container(data);

    // Init Swiper
}
init(scope, csn_editorial.specModule)