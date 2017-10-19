﻿import React from 'react'
import { connect } from 'react-redux'
import { Dispatch } from 'redux';
import INavMenuHeader from 'iNav/Components/iNavMenuHeader'
import INavNodesContainer from 'iNav/Containers/iNavNodesContainer'
import { State, INode } from 'iNav/Types'
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import UI from 'ReactReduxUI'
import KeywordSearch from 'iNav/Components/iNavKeywordSearch'
import {reset} from 'redux-form';

if (!SERVER) {
  require('iNav/Css/iNav.scss')  
}

interface IINavNodes {
  nodes: INode[], 
  activeItemId: number | null
  keywordSearchIsActive: boolean
  cancel: ()=>Dispatch<Actions>
}

//Wrapper component
class INav extends React.Component<IINavNodes> {

  //TODO: add sticky Nav
  componentDidMount(){}
  componentWillUnmount(){}

  render(){
    return (
      <div className={['iNav', this.props.activeItemId !== null || this.props.keywordSearchIsActive ? 'isActive' : ''].join(' ')} onClick={()=>{if(this.props.activeItemId !== null || this.props.keywordSearchIsActive){this.props.cancel()}}}>
        <div className="iNav__container" onClick={(e)=>{e.stopPropagation()}}>
          <KeywordSearch keywordSearchIsActive={this.props.keywordSearchIsActive}/>
          {/* This click handler is to prevent the click event propigating and triggering the cancel fn */}
          <div>
            <INavMenuHeader nodes={this.props.nodes} />
            <INavNodesContainer nodes={this.props.nodes} activeItemId={this.props.activeItemId} />
          </div>
        </div> 
      </div>
    )
  }
}
// Redux Connect
const mapStateToProps = (state: State) => {
  return {
    nodes: state.store.listings.navResults.iNav.nodes
  }
}

const mapDispatchToProps = (dispatch: any) => {
  return {
    cancel: ()=>{
      dispatch([
        {type: ActionTypes.UI.CANCEL},
        reset('keywordSearch')
      ])
    }
  }
}

const componentRootReducer = (initUIState: any) => (state = initUIState, action: Actions) => {
  switch (action.type) {
    case ActionTypes.UI.TOGGLE_IS_ACTIVE:
      return {
        ...state,
        activeItemId: action.payload.isActive ? action.payload.id : null,
      }
    case ActionTypes.UI.FOCUS:
      if(action.meta.form === 'keywordSearch' && action.meta.field === 'keyword') {
        return {
          ...state,
          keywordSearchIsActive: true,
          activeItemId: null          
        }
      } else {
        return state
      }
    case ActionTypes.UI.CANCEL:
    case ActionTypes.UI.CLOSE_INAV:    
      return {
        ...state,
        activeItemId: null,
        keywordSearchIsActive: false       
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
    activeItemId: null,
    keywordSearchIsActive: false
  }
})(INav))
