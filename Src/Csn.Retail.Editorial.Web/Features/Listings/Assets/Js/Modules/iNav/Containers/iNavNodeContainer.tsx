import React from 'react'
import { connect } from 'react-redux'
import INavfacet from 'iNav/Components/iNavFacet'
import { INode } from 'Redux/iNav/Types'
import UI from 'ReactReduxUI'
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import { Dispatch } from 'redux';



if (!SERVER) {
  require('iNav/Css/iNav.NodesContainer')  
}
interface IINavNodeContainer extends INode {
  index: number
  activeItemId: number
}

// This is separated for displaying sub lists
const INavNodeContainer: React.StatelessComponent<IINavNodeContainer> = ({ facets, index, activeItemId, name }) => (
  <div className={['iNav-category__container', activeItemId === index ? 'isActive' : ''].join(' ')}>
    <div className={['iNav-category__container-page'].join(' ')}>
      <ul className='iNav-category__list'>
        {facets.map((facet) => {
          return <INavfacet key={facet.displayValue} {...facet} />
        })}
      </ul>
    </div>
  </div>
);

const componentRootReducer = (initUIState: any, ownProps: any) => (state: any = initUIState, action: Actions): any => {
  switch (action.type) {
    case ActionTypes.UI.INCREMENT:
      if (action.payload.name === ownProps.name)
      return {
        ...state,
        internalItemsCount: state.internalItemsCount + 1
      }
    case ActionTypes.UI.DECREMENT:
      return {
        ...state,
        internalItemsCount: state.internalItemsCount <= 0 ? 0 : state.internalItemsCount - 1
      }
    default:
      return state
  }
}

// Connect the Component to the store
export default (UI({
  key: (props)=>`ui/INavNodeContainer_${props.name}`,
  state: (props)=>({
    internalItemsCount: 0
  }),
  reducer: componentRootReducer,
  mapDispatchToProps: (dispatch: Dispatch<Actions>, ownProps) => ({
    increment: ()=>dispatch({
      type: ActionTypes.UI.INCREMENT,
      payload: {
        name: ownProps.name
      }
    }),
    decrement: ()=>dispatch({
      type: ActionTypes.UI.DECREMENT,
      payload: {
        name: ownProps.name
      }
    })    
  })
})(INavNodeContainer))