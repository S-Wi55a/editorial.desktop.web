import React from 'react'
import { connect } from 'react-redux'
import { createSelector } from 'reselect'
import INavMenuHeader from 'iNav/Components/iNavMenuHeader'
import { ActionTypes } from 'Redux/iNav/Actions/actions'
import { local as ui } from 'redux-fractal';
import { createStore, compose } from 'redux'

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
const mapStateToProps = (state: any) => {
  return {
    nodes: getFilteredNodes(state.iNav)
  }
}

const componentRootReducer = (state: any, action: any): any => {
  // state represents *only* the UI state for this component's scope - not any children
  switch (action.type) {
    case ActionTypes.TOGGLE_IS_ACTIVE:
      return {
        ...state,
        activeItemId: action.payload.id
      }
    default:
      return state
  }
}

const wrapper = compose(

  connect(
    mapStateToProps
  ),

  ui({
    key: 'iNavNodesContainer',
    createStore: (props: any) => {
      return createStore(componentRootReducer, { activeItemId: null }) // NOTE: if list dynamically changes, id may be incorrect as id is set only on inital state.
    },
    filterGlobalActions: (action: any) => {
      const allowedActions = [ActionTypes.TOGGLE_IS_ACTIVE];
      return allowedActions.indexOf(action.type) !== -1;
    }
  })
);

const INavNodesContainer = wrapper(iNavNodes)

export default INavNodesContainer

