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
    cancel?: () => void,
    showText: string,
    cancelText: string,
    articleText: string,
    uiCulture: string
}

const INavConfirmCancelBar: React.StatelessComponent<IINavConfirmCancelBar> = ({ count, fetchINavAndResults, pendingAction, cancel, showText, cancelText, articleText, uiCulture}) => (
    <div className='iNav-category__confirmCancelBar confirmCancelBar'>
        <div className='confirmCancelBar__button confirmCancelBar__button--cancel' onClick={cancel} data-webm-clickvalue={`cancel`}>{cancelText}</div>
        <a className='confirmCancelBar__button confirmCancelBar__button--show' href={pendingAction} data-webm-clickvalue={`show`}
            onClick={(e: any)=>{
                e.preventDefault()
                fetchINavAndResults()
            }
            }>{showText} {count.toLocaleString(uiCulture)} {articleText}{count > 1 ? 's' : ''}</a>      
    </div>
)

const mapStateToProps = (state: any, ownProps: IINavConfirmCancelBar) => {
    return {
        count: typeof state.store.nav.navResults.iNav.pendingQueryCount !== 'undefined' ? state.store.nav.navResults.iNav.pendingQueryCount : state.store.nav.navResults.count,
        pendingAction: typeof state.store.nav.navResults.iNav.pendingUrl !== 'undefined' ? state.store.nav.navResults.iNav.pendingUrl : state.store.nav.navResults.iNav.currentUrl,
        showText: state.store.nav.navResults.iNav.navLabels.navShowText,
        cancelText: state.store.nav.navResults.iNav.navLabels.navCancelText,
        articleText: state.store.nav.navResults.iNav.navLabels.navArticleText,
        uiCulture: state.store.nav.navResults.iNav.navLabels.uiCulture
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