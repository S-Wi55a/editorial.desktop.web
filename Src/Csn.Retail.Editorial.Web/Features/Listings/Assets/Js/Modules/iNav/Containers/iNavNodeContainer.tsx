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
  isLoading: boolean
}

interface IINavList extends ICategory {
  facets: IFacet[]
  name?: string
  displayName: string
  isRefinement?: boolean
}

const INavList: React.StatelessComponent<IINavList> = (props) => (
  <div className={[
      'iNav-category__list-wrapper',
      (props.refinementId === props.id) ? 'iNav-category__list-wrapper--isVisible' : '',
      (props.isLoading) ? 'iNav-category__list-wrapper--isLoading' : ''
      ].join(' ') }>
    {/* Show back button if level 2 or 3 */}
    {props.isRefinement && <div className='iNav-category__back-button' onClick={props.switchPageBack}>{props.displayName}</div>}
    <ul className='iNav-category__list'>
      {props.facets.map((facet, index) => {
        return <INavfacet
          key={facet.displayValue}
          {...props}
          {...facet}
          aspect={props.name}
          id={index}
          />
      })}
    </ul>
  </div>
)

interface IINavNodeContainer extends INode, ICategory {
  index: number
  activeItemId: number
  activePage: number
  refinementId: number | null,
  isLoading: boolean
}

// This is separated for displaying sub lists
const INavNodeContainer: React.StatelessComponent<IINavNodeContainer> = (props) => {

  return <div 
          className={['iNav-category__container', props.activeItemId === props.index ? `iNav-category__container--isActive iNav-category__container--${props.activeItemId}` : ''].join(' ')}
          data-webm-section={props.displayName}
          >
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
          currentRefinement={props.currentRefinement}
          isLoading={props.isLoading}
          />
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
              isLoading={props.isLoading}
              />
          }
        })
      }



    </div>
    <INavConfirmCancelBar index={props.index}/>
  </div>
};

// Connect
const mapStateToProps = (state: any, ownProps: IINavNodeContainer) => {
  return {
      pendingQuery: typeof state.store.listings.navResults.iNav.pending !== 'undefined' ? state.store.listings.navResults.iNav.pending.url : '',
      currentRefinement: state.store.listings.navResults.currentRefinement,
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

// UI Reducer
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
    // Reset the page back to start when menu is cancelled
    case ActionTypes.UI.CANCEL:
    case ActionTypes.UI.TOGGLE_IS_ACTIVE:
    case ActionTypes.UI.CLOSE_INAV:            
      return {
        ...state,
        activePage: 1
        }
    case ActionTypes.API.INAV.FETCH_QUERY_REQUEST:
    case ActionTypes.API.ASPECT.FETCH_QUERY_REQUEST:
    case ActionTypes.API.REFINEMENT.FETCH_QUERY_REQUEST:
      return {
        ...state,
        isLoading: true
        }
    case ActionTypes.API.INAV.FETCH_QUERY_SUCCESS:
    case ActionTypes.API.INAV.FETCH_QUERY_FAILURE:
    case ActionTypes.API.ASPECT.FETCH_QUERY_SUCCESS:
    case ActionTypes.API.ASPECT.FETCH_QUERY_FAILURE:
    case ActionTypes.API.REFINEMENT.FETCH_QUERY_SUCCESS:
    case ActionTypes.API.REFINEMENT.FETCH_QUERY_FAILURE:
      return {
        ...state,
        isLoading: false
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
    refinementId: null,
    isLoading: false
  }
})(INavNodeContainer))