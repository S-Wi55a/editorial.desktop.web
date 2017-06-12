import React from 'react'
import { connect } from 'react-redux'
import ui from 'redux-ui';
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import SearchBar from 'Js/Modules/Redux/iNav/Components/searchBar'


function getiNavNodes(iNav) {

    //Check if it has sub categories
    //TODO: add checks for iNav.iNav.nodes
    const nodesfiltered = iNav.iNav.nodes.filter(function (node) {
        return !!node.facets 
    })

    return nodesfiltered
}


const mapStateToProps = (state) => {
    return {
        nodes: getiNavNodes(state.iNav)
    }
}

const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        toggleSelected: (isSelected, node, facet) => {
            dispatch([
                Actions.toggleIsSelected(isSelected, node, facet),
                Actions.updateQuery()
                ])
        },
        toggleIsActive: () => {
            dispatch(Actions.toggleIsActive(ownProps.uiKey, 'isActive', !ownProps.ui.isActive))
        }
    }
}


let SearchBarContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(SearchBar)

//Wrap UI
SearchBarContainer = ui({
    key: 'searchBar',
    state: {
        isActive: false,
    }
})(SearchBarContainer);


export default SearchBarContainer