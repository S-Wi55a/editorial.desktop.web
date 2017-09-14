import React from 'react'
import { connect } from 'react-redux'
import INavMenuHeader from 'iNav/Components/iNavMenuHeader'
import INavNodeContainer from 'iNav/Containers/iNavNodeContainer'
import * as iNavTypes from 'Redux/iNav/Types'
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import UI from 'ReactReduxUI'


//Wrapper component
const iNavNodes:React.StatelessComponent<{nodes:iNavTypes.INode[]}>  = ({ nodes }) => {
  return (
  <div>
    <INavMenuHeader nodes={nodes} />
    <INavNodeContainer nodes={nodes} />
  </div>
  )
}
// Redux Connect
const mapStateToProps = (state:iNavTypes.State) => {
  return {
    nodes: state.iNav.iNav.nodes
  }
}

const componentRootReducer = (initUIState: any) => (state = initUIState, action: Actions): any => {
  switch (action.type) {
    case ActionTypes.UI.TOGGLE_IS_ACTIVE:
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
