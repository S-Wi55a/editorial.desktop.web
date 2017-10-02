import React from 'react'
import { connect } from 'react-redux'
import INavfacet from 'iNav/Components/iNavFacet'
import INavConfirmCancelBar from 'iNav/Components/iNavConfirmCancelBar'
import { State, INode, IFacet } from 'iNav/Types'
import UI from 'ReactReduxUI'
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import { Dispatch } from 'redux';

if (!SERVER) {
  require('iNav/Css/iNav.NodesContainer')  
}

interface ICategory {
  pendingQuery?: string
  currentRefinement?: string
  switchPageBack?: (a: any)=>any
  refinementId?: number  
  id?: number
}

interface IINavList extends ICategory {
  facets: IFacet[]
  name?: string
  displayName: string
  isRefinement?: boolean
}

const INavList: React.StatelessComponent<IINavList> = (props) => (
  <div className={['iNav-category__list-wrapper', (props.refinementId === props.id) ? 'iNav-category__list-wrapper--isVisible' : ''].join(' ') }>
    {/* Show back button if level 2 or 3 */} 
    {props.isRefinement && <div className='iNav-category__back-button' onClick={props.switchPageBack}>{props.displayName}</div>}
    <ul className='iNav-category__list'>
      {props.facets.map((facet, index) => {
        return <INavfacet 
          key={facet.displayValue} 
          {...props}
          {...facet}
          aspect={props.name} 
          pendingQuery={props.pendingQuery} 
          currentRefinement={props.currentRefinement} 
          id={index} 
          isRefinement={props.isRefinement}
          />
      })}
    </ul>
  </div>
)

interface IINavNodeContainer extends INode, ICategory {
  index: number
  activeItemId: number
  activePage: number
}

// This is separated for displaying sub lists
const INavNodeContainer: React.StatelessComponent<IINavNodeContainer> = (props) => {
//debugger
  return <div className={['iNav-category__container', props.activeItemId === props.index ? 'isActive' : ''].join(' ')}>
    <div className='iNav-category__header'>{props.displayName}</div>
    <div className={['iNav-category__container-page', `iNav-category__container-page--${props.activePage}`].join(' ')}>
      {/* First Level*/}
      <INavList {...props}/>
      {/* Second Level*/}
      { props.refinements && 
        <INavList 
          {...props.refinements} 
          name={props.name} 
          isRefinement={true} 
          switchPageBack={props.switchPageBack} 
          pendingQuery={props.pendingQuery} 
          currentRefinement={props.currentRefinement}/>
      }
      {/* Third Level*/}
      { props.refinements && props.refinements.facets && props.refinements.facets.map((facet, index)=>{
          if(facet.isRefineable){
            return <INavList 
              key={facet.displayValue}
              {...props.refinements}
              {...facet.refinements}
              name={props.name} 
              refinementId={props.refinementId}  
              isRefinement={true} 
              switchPageBack={props.switchPageBack} 
              pendingQuery={props.pendingQuery} 
              currentRefinement={props.currentRefinement} 
              id={index}
              />
          }
        })
      }



    </div>
    <INavConfirmCancelBar index={props.index}/>
  </div>
};

const mapStateToProps = (state: State, ownProps: IINavNodeContainer) => {
  return {
      pendingQuery: state.iNav.pendingQuery,
      currentRefinement: state.iNav.currentRefinement,
      refinementId: state[`ui/iNavNodeContainer${ownProps.name}`] ?  state[`ui/iNavNodeContainer${ownProps.name}`].refinementId : null
  }
}

const mapDispatchToProps = (dispatch: any) => {
  return {
    switchPageBack: () => {
      dispatch(
        { type: ActionTypes.UI.SWITCH_PAGE_BACK }
      )
    }
  }
}

const componentRootReducer = (initUIState: any) => (state = initUIState, action: Actions) => {
  switch (action.type) {
    case ActionTypes.UI.SWITCH_PAGE_FORWARD:
      return {
        ...state,
        activePage: state.activePage + 1,
        refinementId: action.payload ? action.payload.refinementId : null
      }
    case ActionTypes.UI.SWITCH_PAGE_BACK:
      return {
        ...state,
        activePage: state.activePage > 1 ? state.activePage - 1 : 1
      }
    case ActionTypes.UI.CANCEL:
    case ActionTypes.UI.CLOSE_INAV: 
      return {
        ...state,
        activePage: 1
      }
    default:
      return state
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(UI({
  key: (props: IINavNodeContainer)=>`ui/iNavNodeContainer${props.name}`,  
  reducer: componentRootReducer,
  state: {
    activePage: 1,
    refinementId: null
  }
})(INavNodeContainer))