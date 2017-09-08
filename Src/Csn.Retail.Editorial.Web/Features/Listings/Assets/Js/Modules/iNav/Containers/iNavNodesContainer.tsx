﻿import React from 'react'
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

const INavNodesContainer =   connect(
  mapStateToProps
)(iNavNodes)

export default INavNodesContainer

