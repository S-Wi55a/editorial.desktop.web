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
    fetchINav?: (q?: string)=>void
    cancel?: ()=>void
}
//TODO: how to get hardcoded words data driven?
const INavConfirmCancelBar: React.StatelessComponent<IINavConfirmCancelBar> = ({count, fetchINav, pendingQuery, cancel}) => (
    <div className='iNav-category__confirmCancelBar confirmCancelBar'>
        <div className='confirmCancelBar__button confirmCancelBar__button--cancel' onClick={cancel}>Cancel</div>
        <a className='confirmCancelBar__button confirmCancelBar__button--show' href={iNav.home(pendingQuery)} 
            onClick={(e)=>{
                e.preventDefault()
                fetchINav()
            }
        }>Show {count} Articles</a>      
    </div>
)

const mapStateToProps = (state: any, ownProps: IINavConfirmCancelBar) => {
    return {
        count: typeof state.store.listings.navResults.iNav.nodes[ownProps.index].count !== 'undefined' ? state.store.listings.navResults.iNav.nodes[ownProps.index].count : state.store.listings.navResults.count,
        pendingQuery: typeof state.store.listings.pendingQuery !== 'undefined' ? state.store.listings.pendingQuery : '' 
    }
}

const mapDispatchToProps = (dispatch: any) => {
    return {
        fetchINav: (query?: string)=>dispatch([
            Thunks.fetchINav(query),
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