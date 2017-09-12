import React from 'react'
import { connect } from 'react-redux'
import { createSelector } from 'reselect'
import INavMenuHeader from 'iNav/Components/iNavMenuHeader'
import { ActionTypes } from 'Redux/iNav/Actions/actions'
import { local as ui } from 'redux-fractal';
import { createStore, compose } from 'redux';

//Wrapper component
const iNavNodes = ({ nodes }) => {
    return <INavMenuHeader nodes={nodes} />    
}

//Selectors
const getiNavNodes = (iNav) => iNav.iNav.nodes

const getFilteredNodes =
    (iNavNodes) => {
        //Check if it has sub categories
        //TODO: add checks for iNav.iNav.nodes - right now we just expect the data to be correct
        const nodesfiltered = iNavNodes.filter(function (node) {
            return !!node.facets 
        })

        return nodesfiltered
    }

// Redux Connect
const mapStateToProps = (state: any) => {
    return {
        nodes: getFilteredNodes(state.iNav.iNav.nodes)
    }
}

const wrapper = compose(
    
    connect(
        mapStateToProps
    ),

    ui({
        key: 'iNavNodesContainer',
        createStore: (props: any) => {
            return createStore(rootReducer, { isVisibleId: null })
        },
        filterGlobalActions: (action: any) => {
            // Any logic to determine if the actions should be forwarded
            // to the component's reducer. By default none is except those
            // originated by component itself
            const allowedActions = [ActionTypes.TOGGLE_IS_ACTIVE];
            return allowedActions.indexOf(action.type) !== -1;
        }
    })
);

const INavNodesContainer = wrapper(iNavNodes)
    
export default INavNodesContainer

const rootReducer = (state: any, action: any): any => {
        // state represents *only* the UI state for this component's scope - not any children
        switch (action.type) {
        case ActionTypes.TOGGLE_IS_ACTIVE:
            return {
                ...state,
                isVisibleId: action.payload.id
            }
        default:
            return state
        }
    }