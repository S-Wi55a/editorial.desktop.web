import React from 'react'
import { connect } from 'react-redux'
import ui from 'redux-ui';
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import SearchBar from 'Js/Modules/Redux/iNav/Components/searchBar'

function getiNavNodes(iNav) {

    //Check if it has sub categories
    //TODO: add checks for iNav.iNav.nodes - right now we just expect the data to be correct
    const nodesfiltered = iNav.iNav.nodes.filter(function (node) {
        return !!node.facets 
    })

    return nodesfiltered
}

// Redux Connect
const mapStateToProps = (state) => {
    return {
        nodes: getiNavNodes(state.iNav)
    }
}

const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        toggleSelected: (isSelected, node, facet, query) => {
            dispatch([
                Actions.toggleIsSelected(isSelected, node, facet),
                Actions.fetchQueryRequest(query)
                ])
        },
        toggleIsActive: () => {
            dispatch(Actions.toggleIsActive(ownProps.uiKey, 'isActive', !ownProps.ui.isActive))
        }
    }
}

// Connect the Component to the store
let SearchBarContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(SearchBar)

// Add the UI to the store
// This must be after Connect because we use the ui props to in connect
SearchBarContainer = ui({
    key: 'searchBar',
    state: {
        isActive: false,
    }
})(SearchBarContainer);


export default SearchBarContainer