import * as React from "react"
import { connect } from 'react-redux'
import { Dispatch } from 'redux';
import { createStore, compose } from 'redux'
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import * as iNavTypes from 'Redux/iNav/Types'
import UI from 'ReactReduxUI'
import { State } from 'Redux/iNav/Types'


interface IINavMenuHeaderItemComponent {
  ui: any
  node: iNavTypes.INode
  toggleIsSelected: (id: number, isActive: boolean) => Actions
  index: number
  isActive: boolean
  count: number
}

const INavMenuHeaderItemComponent: React.StatelessComponent<IINavMenuHeaderItemComponent> = ({ isActive, node, toggleIsSelected, index, count }) => {
  return (
    <div className={['iNav__menu-header-item', isActive ? 'isActive' : ''].join(' ')} onClick={() => toggleIsSelected(index, isActive)}>
      {node.displayName} {count ? `(${count})` : ''}
    </div>
  )
}

// Redux Connect

const mapStateToProps = (state: State, ownProps: IINavMenuHeaderItemComponent) => {
  return {
    count: state[`ui/INavNodeContainer_${ownProps.node.name}`] ? state[`ui/INavNodeContainer_${ownProps.node.name}`].internalItemsCount : 0
  }
}

const mapDispatchToProps = (dispatch: Dispatch<Actions>) => {
  return {
    toggleIsSelected: (id: number, isActive: boolean) => {
      dispatch({
        type: ActionTypes.UI.TOGGLE_IS_ACTIVE,
        payload: {
          id,
          isActive: !isActive
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
        isActive: action.payload.isActive && state.id === action.payload.id
      }
    case ActionTypes.UI.CANCEL:
    case ActionTypes.UI.CLOSE_INAV:
      return {
        ...state,
        isActive: false        
      }
    default:
      return state
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(UI({
  key: (props: IINavMenuHeaderItemComponent)=>`ui/INavMenuHeaderItemComponent_${props.node.name}`,  
  state: (props: IINavMenuHeaderItemComponent)=>({
    id: props.index, 
    isActive: false,
  }),
  reducer: componentRootReducer
})(INavMenuHeaderItemComponent))