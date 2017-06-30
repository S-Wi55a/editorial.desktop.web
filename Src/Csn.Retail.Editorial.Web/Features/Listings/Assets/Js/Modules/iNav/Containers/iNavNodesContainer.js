import React from 'react'
import { connect } from 'react-redux'
import { createSelector } from 'reselect'
import INavNodeContainer from 'Js/Modules/iNav/Containers/iNavNodeContainer'
import Collapse,  { Panel } from 'rc-collapse';

//Wrapper component
const iNavNodes = ({ nodes }) => {

    return (
        <Collapse accordion={true} defaultActiveKey={'0'}>
            {nodes.map((node, index) => {
                return (
                    <Panel key={index} header={node.displayName} showArrow={false}>
                        <INavNodeContainer node={node} />
                    </Panel>
                    )
            })}
        </Collapse>
    )
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
const INavNodesContainer = connect(
    mapStateToProps
)(iNavNodes)

export default INavNodesContainer