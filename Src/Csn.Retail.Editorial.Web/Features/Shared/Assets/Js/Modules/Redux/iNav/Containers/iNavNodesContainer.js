import React from 'react'
import { connect } from 'react-redux'
import { createSelector } from 'reselect'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import INavNodeContainer from 'Js/Modules/Redux/iNav/Containers/iNavNodeContainer'

//Wrapper component
const iNavNodes = ({nodes}) => (
    <div>
        {nodes.map((node) => {
            return (<INavNodeContainer key={node.displayName} node={node} />)
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
        nodes: getFilteredNodes(state.iNav),
    }
}

// Connect the Component to the store
const INavNodesContainer = connect(
    mapStateToProps
)(iNavNodes)

export default INavNodesContainer