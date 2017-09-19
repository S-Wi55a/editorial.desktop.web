import React from 'react'
import { connect } from 'react-redux'
import { Dispatch } from 'redux';
import INavMenuHeader from 'iNav/Components/iNavMenuHeader'
import INavNodesContainer from 'iNav/Containers/iNavNodesContainer'
import { State, INode } from 'Redux/iNav/Types'
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import UI from 'ReactReduxUI'

if (!SERVER) {
  require('iNav/Css/iNav.scss')  
}

interface IINavNodes {
  nodes: INode[], 
  activeItemId: number | null
  cancel: ()=>Dispatch<Actions>
}

//Wrapper component
const INav:React.StatelessComponent<IINavNodes>  = ({ nodes, activeItemId, cancel }) => {
  return (
    <div className={['iNav', activeItemId !== null  ? 'isActive' : ''].join(' ')} onClick={()=>activeItemId !== null && cancel()}>
      <div className="iNav__container">
        <div onClick={(e)=>{e.stopPropagation()}}>
        <INavMenuHeader nodes={nodes} />
        {/* This click handler is to prevent the clicke event propigating nad triggering the cancel fn */}
        <INavNodesContainer nodes={nodes} activeItemId={activeItemId} />
        </div>
      </div> 
    </div>
  )
}
// Redux Connect
const mapStateToProps = (state: State) => {
  return {
    nodes: state.iNav.iNav.nodes
  }
}

const mapDispatchToProps = (dispatch: Dispatch<Actions>) => {
  return {
    cancel: ()=>{
      dispatch({type: ActionTypes.UI.CANCEL})
    }
  }
}

const componentRootReducer = (initUIState: any) => (state = initUIState, action: Actions) => {
  switch (action.type) {
    case ActionTypes.UI.TOGGLE_IS_ACTIVE:
      return {
        ...state,
        activeItemId: action.payload.isActive ? action.payload.id : null        
      }
    case ActionTypes.UI.CANCEL:
      return {
        ...state,
        activeItemId: null        
      }
    default:
      return state
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(UI({
  key: 'ui/INavNodes',
  reducer: componentRootReducer,
  state: {
    activeItemId: null
  }
})(INav))
