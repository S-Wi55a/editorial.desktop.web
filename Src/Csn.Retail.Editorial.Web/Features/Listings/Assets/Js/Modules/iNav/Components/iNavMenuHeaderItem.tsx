import * as React from "react"
import { connect } from 'react-redux'
import { Dispatch } from 'redux';
import { createStore, compose } from 'redux';
import { ActionTypes } from 'Redux/iNav/Actions/actions'
import { injectAsyncReducer } from 'Redux/Global/Store/store.server.js'
import UI from 'ui'

interface IINavMenuHeaderItemComponent {
  ui: any,
  node: any //TODO: update
  toggleIsSelected: (id: number, isActive: boolean) => any
  index: number,
  isActive: boolean  
}

const INavMenuHeaderItemComponent: React.StatelessComponent<IINavMenuHeaderItemComponent> = ({ isActive, node, toggleIsSelected, index }) => {

  return (
    <div className={['iNav__menu-header-item', isActive && 'isActive'].join(' ')} onClick={() => toggleIsSelected(index, isActive)}>
      {node.displayName}
    </div>
  )
}

const mapDispatchToProps = (dispatch: Dispatch<any>) => {
  return {
    toggleIsSelected: (id: number, isActive: boolean) => {
      dispatch({
        type: ActionTypes.TOGGLE_IS_ACTIVE,
        payload: {
          id,
          isActive
        }
      })
    }
  }
}

// const initUIState = {
//   id: props.index, 
//   isActive: false
// }

const componentRootReducer = (state: any = null, action: any): any => {
  // state represents *only* the UI state for this component's scope - not any children
  switch (action.type) {
    case ActionTypes.TOGGLE_IS_ACTIVE:
      return {
        ...state,
        isActive: !action.payload.isActive && state.id === action.payload.id
      }
    default:
      return state
  }
}

// //TODO: wrap in HMR
// injectAsyncReducer(global.store, `ui/iNavNodesContainerItem_${props.index}`, componentRootReducer);

//export default connect(null,mapDispatchToProps)(INavMenuHeaderItemComponent)


export default connect(null,mapDispatchToProps)(UI({
  key: (props)=>props.node.name,
  reducer: componentRootReducer
})(INavMenuHeaderItemComponent))