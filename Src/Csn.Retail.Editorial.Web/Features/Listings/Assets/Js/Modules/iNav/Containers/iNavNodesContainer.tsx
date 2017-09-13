import React from 'react'
import { connect } from 'react-redux'
import INavMenuHeader from 'iNav/Components/iNavMenuHeader'
import { ActionTypes } from 'Redux/iNav/Actions/actions'
import { injectAsyncReducer } from 'Redux/Global/Store/store.server.js'

import UI from 'ui'


// TODO: add node Interface

//Wrapper component
const iNavNodes:React.StatelessComponent<{nodes:any}>  = ({ nodes }) => {
  return <INavMenuHeader nodes={nodes} />
}
// Redux Connect
const mapStateToProps = (state: any) => {
  return {
    nodes: state.iNav.iNav.nodes
  }
}

const initUIState = {
  activeItemId: null
}

const componentRootReducer = (state: any = initUIState, action: any): any => {
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

//TODO: wrap in HMR
//injectAsyncReducer(global.store, 'ui/iNavNodes', componentRootReducer);




export default connect(mapStateToProps)(UI({
  key: 'ui/iNavNodes',
  reducer: componentRootReducer
})(iNavNodes))
