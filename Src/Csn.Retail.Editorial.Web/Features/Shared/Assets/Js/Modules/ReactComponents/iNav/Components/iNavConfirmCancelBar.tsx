import React from 'react'
import { connect } from 'react-redux'
import { Actions, ActionTypes, Thunks } from '../Actions/actions'
import { State } from '../Types'


if (!SERVER) {
    require('../Css/iNav.ConfirmCancelBar')  
  }


interface IINavConfirmCancelBar {
    index?: number
    count?: number
    pendingAction?: string
    fetchINavAndResults?: (q?: string)=>void
    cancel?: ()=>void
}
//TODO: how to get hardcoded words data driven?
const INavConfirmCancelBar: React.StatelessComponent<IINavConfirmCancelBar> = ({ count, fetchINavAndResults, pendingAction, cancel}) => (
    <div className='iNav-category__confirmCancelBar confirmCancelBar'>
        <div className='confirmCancelBar__button confirmCancelBar__button--cancel' onClick={cancel} data-webm-clickvalue={`cancel`}>Cancel</div>
        <a className='confirmCancelBar__button confirmCancelBar__button--show' href={pendingAction} data-webm-clickvalue={`show`}
            onClick={(e: any)=>{
                e.preventDefault()
                fetchINavAndResults()
            }
        }>Show {count.toLocaleString()} Article{count > 1 ? 's' : ''}</a>      
    </div>
)

const mapStateToProps = (state: any, ownProps: IINavConfirmCancelBar) => {
    return {
        count: typeof state.store.nav.navResults.iNav.pendingQueryCount !== 'undefined' ? state.store.nav.navResults.iNav.pendingQueryCount : state.store.nav.navResults.count,
        pendingAction: typeof state.store.nav.navResults.iNav.pendingUrl !== 'undefined' ? state.store.nav.navResults.iNav.pendingUrl : state.store.nav.navResults.iNav.currentUrl,
    }
}

const mapDispatchToProps = (dispatch: any) => {
    return {
        fetchINavAndResults: (query?: string)=>dispatch([
            Thunks.fetchINavAndResults(query),
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