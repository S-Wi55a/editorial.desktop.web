import React from 'react'
import { connect } from 'react-redux'
import { createSelector } from 'reselect'
import INavMenuHeader from 'Js/Modules/iNav/Components/iNavMenuHeader'
import ui from 'redux-ui'
import * as iNav from 'Js/Modules/Redux/iNav/Actions/actionTypes'

//Wrapper component
const iNavNodes = ({ nodes }) => {
    return <INavMenuHeader nodes={nodes} />    
}

//Selectors
const getiNavNodes = (iNav) => iNav.iNav.nodes

const getFilteredNodes = createSelector(
    getiNavNodes,
    (iNavNodes) => {
        //Check if it has sub categories
        //TODO: add checks for iNav.iNav.nodes - right now we just expect the data to be correct
        const nodesfiltered = iNavNodes.filter(function (node) {
            return !!node.facets 
        })

        return nodesfiltered
    }
)

// Redux Connect
const mapStateToProps = (state) => {
    return {
        nodes: getFilteredNodes(state.iNav),
    }
}

// Connect the Component to the store
const INavNodesContainerConnect = connect(
    mapStateToProps
)(iNavNodes)


// Add the UI to the store
const INavNodesContainer = ui({
    key: 'iNavNodesContainer',
    state: {
        isVisible: 0 // ID because some buttons can contain multiple items
    },
    // customReducer: you can handle the UI state for this component's scope by dispatching actions
    reducer: (state, action) => {
        // state represents *only* the UI state for this component's scope - not any children
        switch (action.type) {
        case iNav.TOGGLE_IS_ACTIVE:
            return state.set('isActive', !state.get('isActive'))
        default:
            return state
        }
    }
  })(INavNodesContainerConnect);

export default INavNodesContainer