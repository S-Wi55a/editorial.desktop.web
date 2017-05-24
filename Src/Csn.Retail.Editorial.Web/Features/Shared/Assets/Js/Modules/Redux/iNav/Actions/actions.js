import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes.js'


// Toggle IsSelected
export const toggleIsSelected = (isSelected, node, facet) => ({
    type: ActionTypes.TOGGLE_SELECTED,
    isSelected,
    node, //is string but can probably be changed to Id
    facet //is string but can probably be changed to Id
})







//var t = {
    //type: 'SGN_TOGGLE_SELECTED',
    //isSelected: true,
    //node:'ArticleTypes', 
    //facet:'Review'
//}