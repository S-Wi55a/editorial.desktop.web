import React from 'react'
import { connect } from 'react-redux'
import { Dispatch } from 'redux'
import { Actions, ActionTypes, Thunks } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'
import { State } from 'iNav/Types'


if (!SERVER) {
    require('iNav/Css/iNav.ConfirmCancelBar')  
  }


interface IINavConfirmCancelBar {
    index?: number
    count?: number
    pendingQuery?: string
    fetchSearchResults?: ()=>void
    cancel?: ()=>void
}
//TODO: how to get hardcoded words data driven?
const INavConfirmCancelBar: React.StatelessComponent<IINavConfirmCancelBar> = ({count, fetchSearchResults, pendingQuery, cancel}) => (
    <div className='iNav-category__confirmCancelBar confirmCancelBar'>
        <div className='confirmCancelBar__button confirmCancelBar__button--cancel' onClick={cancel}>Cancel</div>
        <a className='confirmCancelBar__button confirmCancelBar__button--show' href={iNav.home(pendingQuery)} 
            onClick={(e)=>{
                e.preventDefault()
                fetchSearchResults()
            }
        }>Show {count} Articles</a>      
    </div>
)

const mapStateToProps = (state: State, ownProps: IINavConfirmCancelBar) => {
    return {
        count: typeof state.iNav.iNav.nodes[ownProps.index].count !== 'undefined' ? state.iNav.iNav.nodes[ownProps.index].count : state.iNav.count,
        pendingQuery: state.iNav.pendingQuery ? state.iNav.pendingQuery : '' 
    }
}

const mapDispatchToProps = (dispatch: any) => {
    return {
        fetchSearchResults: ()=>dispatch([
            Thunks.fetchINav(),
            {type:ActionTypes.UI.CLOSE_INAV}
        ]),
        cancel: ()=>{
            dispatch({type: ActionTypes.UI.CANCEL})
          }
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(INavConfirmCancelBar)