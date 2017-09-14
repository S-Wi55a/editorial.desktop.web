import * as React from "react"
import { connect } from 'react-redux'
import { Dispatch } from 'redux';
import { createStore, compose } from 'redux';
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import * as iNavTypes from 'Redux/iNav/Types'
import UI from 'ReactReduxUI'

interface IINavMenuHeaderItemComponent {
  ui: any,
  node: iNavTypes.INode
  toggleIsSelected: (id: number, isActive: boolean) => Actions
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

const mapDispatchToProps = (dispatch: Dispatch<Actions>) => {
  return {
    toggleIsSelected: (id: number, isActive: boolean) => {
      dispatch({
        type: ActionTypes.UI.TOGGLE_IS_ACTIVE,
        payload: {
          id,
          isActive
        }
      })
    }
  }
}

const componentRootReducer = (initUIState: any) => (state: any = initUIState, action: Actions): any => {
  switch (action.type) {
    case ActionTypes.UI.TOGGLE_IS_ACTIVE:
      return {
        ...state,
        isActive: !action.payload.isActive && state.id === action.payload.id
      }
    default:
      return state
  }
}

export default connect(
  null,
  mapDispatchToProps
)(UI({
  key: (props)=>props.node.name,
  state: (props)=>({
    id: props.index, 
    isActive: false 
  }),
  reducer: componentRootReducer
})(INavMenuHeaderItemComponent))