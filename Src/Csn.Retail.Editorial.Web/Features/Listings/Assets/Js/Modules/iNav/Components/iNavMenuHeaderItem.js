import React from 'react'
import { connect } from 'react-redux'
import ui from 'redux-ui'
import * as iNav from 'Js/Modules/Redux/iNav/Actions/actionTypes'

const INavMenuHeaderItemComponent = ({ui, node, toggleIsSelected}) => { 
  
  return (
        <div className={['iNav__menu-header-item', ui.isActive?'isActive':''].join(' ')} onClick={toggleIsSelected}>
          {node.displayName}
        </div>
  )    
}

//Connect
const mapDispatchToProps = (dispatch) => {
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
    isActive: (props) => props.index === props.ui.getIn(['iNavNodesContainer', 'isVisible'])
  }
})(INavMenuHeaderItemComponentConnect);

export default INavMenuHeaderItem

