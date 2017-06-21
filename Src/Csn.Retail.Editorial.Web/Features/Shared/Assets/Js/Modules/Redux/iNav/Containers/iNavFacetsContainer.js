﻿import React from 'react'
import { connect } from 'react-redux'
import { createSelector } from 'reselect'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import INavFacet from 'Js/Modules/Redux/iNav/Components/iNavFacet'

//Wrapper component
const iNavFacets = ({nodes, toggleIsSelected}) => (
    <div>
        {nodes.map((node) => {
            return (<INavFacet key={node.displayName} {...node} toggleIsSelected={toggleIsSelected} />)
        })}
    </div>
) 

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
        nodes: getFilteredNodes(state.iNav)
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        toggleIsSelected: (isSelected, node, facet, query) => {
            dispatch([
                Actions.fetchQueryRequest(query),
                Actions.toggleIsSelected(isSelected, node, facet)
            ])
        }
    }
}

// Connect the Component to the store
const INavFacetsContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(iNavFacets)

export default INavFacetsContainer