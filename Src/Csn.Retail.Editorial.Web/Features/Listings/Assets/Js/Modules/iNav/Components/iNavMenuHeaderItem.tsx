import * as React from "react"
import { connect } from 'react-redux'
import { Dispatch } from 'redux';

import ui from 'redux-ui'
import * as iNav from 'Redux/iNav/Actions/actionTypes'

interface IINavMenuHeaderItemComponent {
  ui: any,
  node: any //TODO: update
  toggleIsSelected: any //TODO: update
}

const INavMenuHeaderItemComponent: React.StatelessComponent<IINavMenuHeaderItemComponent> = ({ui, node, toggleIsSelected}) => { 
  
  return (
        <div className={['iNav__menu-header-item', ui.isActive?'isActive':''].join(' ')} onClick={toggleIsSelected}>
          {node.displayName}
        </div>
  )    
}

//Connect
const mapDispatchToProps = (dispatch: any) => {
  return {
      toggleIsSelected: () => {
          dispatch({type:iNav.TOGGLE_IS_ACTIVE})
      }
  }
}

const INavMenuHeaderItemComponentConnect = connect(
  null,
  mapDispatchToProps
)(INavMenuHeaderItemComponent)


// Add the UI to the store
const INavMenuHeaderItem = ui({
  state: {
    isActive: (props: any) => props.index === props.ui.getIn(['iNavNodesContainer', 'isVisible'])
  }
})(INavMenuHeaderItemComponentConnect);

export default INavMenuHeaderItem

