import React from 'react'
import { connect } from 'react-redux'
import INavMenuHeader from 'iNav/Components/iNavMenuHeader'
import { ActionTypes } from 'Redux/iNav/Actions/actions'
import { injectAsyncReducer } from 'Redux/Global/Store/store.server.js'

import UI from 'ReactReduxUI'

// TODO: add node Interface

//Wrapper component
const iNavNodes:React.StatelessComponent<{nodes:any}>  = ({ nodes }) => {
  return <INavMenuHeader nodes={nodes} />
}
// Redux Connect
const mapStateToProps = ({iNav}:any) => {
  return {
    nodes: iNav.iNav.nodes
  }
}


const componentRootReducer = (initUIState: any) => (state: any = initUIState, action: any): any => {
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


export default connect(mapStateToProps)(UI({
  key: 'ui/iNavNodes',
  reducer: componentRootReducer,
  state: {
    activeItemId: null
  }
})(iNavNodes))
