import * as React from "react"
import { connect } from 'react-redux'
import { Dispatch } from 'redux';
import { local as ui } from 'redux-fractal';
import { createStore, compose } from 'redux';
import { ActionTypes } from 'Redux/iNav/Actions/actions'

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

const componentRootReducer = (state: any, action: any): any => {
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

const wrapper = compose(
  ui({
    key: props => `iNavNodesContainerItem_${props.index}`,
    createStore: (props: any) => {
      return createStore(componentRootReducer, { id: props.index, isActive: false }) // NOTE: if list dynamically changes, id may be incorrect as id is set only on inital state.
    },
    mapDispatchToProps: (dispatch: Dispatch<any>) => {
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
    },
    filterGlobalActions: (action: any) => {
      const allowedActions = [ActionTypes.TOGGLE_IS_ACTIVE];
      return allowedActions.indexOf(action.type) !== -1;
    }
  })
)

const INavMenuHeaderItem = wrapper(INavMenuHeaderItemComponent)

export default INavMenuHeaderItem