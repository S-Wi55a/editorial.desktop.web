import * as React from "react"
import { connect } from 'react-redux'
import { Dispatch } from 'redux';
import { local as ui } from 'redux-fractal';
import { createStore, compose } from 'redux';
import { ActionTypes } from 'Redux/iNav/Actions/actions'

interface IINavMenuHeaderItemComponent {
  ui: any,
  node: any //TODO: update
  toggleIsSelected: (id: number) => any
  index: number
}

const INavMenuHeaderItemComponent: React.StatelessComponent<IINavMenuHeaderItemComponent> = ({ isActive, node, toggleIsSelected, index }) => {

  return (
    <div className={['iNav__menu-header-item', isActive && 'isActive'].join(' ')} onClick={() => toggleIsSelected(index)}>
      {node.displayName}{index}
    </div>
  )
}


// const INavMenuHeaderItemComponentConnect = connect(
//   null,
//   null
// )(INavMenuHeaderItemComponent)


// // Add the UI to the store
// const INavMenuHeaderItem = ui({
//   key: 'iNavNodesContainer',
//   mapStateToProps: (componentState: any, ownProps: any) => {
//     //debugger
//     return {
//       isActive: componentState.isVisibleId === ownProps.index
//     }
//   },
//   mapDispatchToProps: (dispatch: Dispatch<any>) => {
//     // ALL actions dispatched via 'dispatch' function above have the component key tagged to the action.
//     // You can see that by inspecting action.meta.reduxFractalTriggerComponent in redux dev tools.
//     // All local actions are dispatched also on the global store
//     // You must return an object containing the keys that will become props to the component just like in react redux 'connect'
//     return {
//       toggleIsSelected: (id: number) => {
//         dispatch({
//           type: ActionTypes.TOGGLE_IS_ACTIVE,
//           payload: {
//             id
//           }
//         })
//       }
//     }
//   },

// })(INavMenuHeaderItemComponentConnect);



// Redux Connect
const mapStateToProps = (state: any) => {
  return {
      ui: state.local['iNavNodesContainer']
  }
}

const wrapper = compose(
  
  connect(
      mapStateToProps
  ),

  ui({
      key: props => `iNavNodesContainerItem_${props.index}`,
      createStore: (props: any) => {
          return createStore(rootReducer, { id: props.index, isActive: false }) // NOTE if list dynamically changes, id may be incorrect unless handles by an action
      },
      mapStateToProps: (componentState: any, ownProps: any) => {
          
          return {
              isActive: ownProps.index === ownProps.ui.isVisibleId
          }
      },
      mapDispatchToProps: (dispatch: Dispatch<any>) => {
        // ALL actions dispatched via 'dispatch' function above have the component key tagged to the action.
        // You can see that by inspecting action.meta.reduxFractalTriggerComponent in redux dev tools.
        // All local actions are dispatched also on the global store
        // You must return an object containing the keys that will become props to the component just like in react redux 'connect'
        return {
          toggleIsSelected: (id: number) => {
            dispatch({
              type: ActionTypes.TOGGLE_IS_ACTIVE,
              payload: {
                id
              }
            })
          }
        }
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

const INavMenuHeaderItem = wrapper(INavMenuHeaderItemComponent)
  
export default INavMenuHeaderItem

const rootReducer = (state: any, action: any): any => {
      // state represents *only* the UI state for this component's scope - not any children
      switch (action.type) {
      case ActionTypes.TOGGLE_IS_ACTIVE:
          return {
              ...state,
              isActive: state.id === action.payload.id
          }
      default:
          return state
      }
  }