import * as React from 'react'
import { connect } from 'react-redux'
import { Actions, ActionTypes } from '../Actions/actions'
import * as iNavTypes from '../Types'
import UI from 'ReactReduxUI'
import { State } from '../Types'


interface IINavMenuHeaderItemComponent {
  ui: any
  node: iNavTypes.INode
  toggleIsSelected: (id: number, isActive: boolean) => Actions
  index: number
  isActive: boolean
  count: number
}

const INavMenuHeaderItemComponent: React.StatelessComponent<IINavMenuHeaderItemComponent> = ({ isActive, node, toggleIsSelected, index, count }) => {
  const handleItemClick = () => {
    scrollNavToTop(isActive);
    toggleIsSelected(index, isActive);
  }

  return (
    <div 
      className={['iNav__menu-header-item', isActive ? 'iNav__menu-header-item--isActive' : ''].join(' ')} 
      onClick={handleItemClick}
      data-webm-section={node.displayName}
      data-webm-clickvalue={node.displayName}
      >
      {node.displayName}{count ? <span className="iNav__menu-header-item-count">{count}</span> : ''}
    </div>
  )
}

const scrollNavToTop = (isActive: boolean) => {
  if(document.querySelector('.landing-page.landing-page--hasHeroImage') && !isActive) { // Only when its landing page with hero image
    const iNav: HTMLElement = document.querySelector('.iNav');
    if(iNav.offsetTop == 0) {
      const scrollToTop = () => {
        var i = 21.5;
        var int = setInterval(function() {
          window.scrollTo(0, i);
          i += 21.5;
          if (i > 215) clearInterval(int);
        }, 20);
      }
      scrollToTop();
    }
  }
}

function findIsSelected(facets: iNavTypes.IFacet[]) {
  let count = 0
  facets.forEach(item => {
    if(item.isSelected) {count++}
  });
  return count
}

// Redux Connect
const mapStateToProps = (state: State, ownProps: IINavMenuHeaderItemComponent) => {
  return {
    count: findIsSelected(state.store.nav.navResults.iNav.nodes[ownProps.index].facets)
  }
}

const mapDispatchToProps = (dispatch: any) => {
  return {
    toggleIsSelected: (id: number, isActive: boolean) => {
        dispatch([
            {
                type: ActionTypes.UI.TOGGLE_IS_ACTIVE,
                payload: {
                    id,
                    isActive: !isActive
                }
            }
        ]);
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