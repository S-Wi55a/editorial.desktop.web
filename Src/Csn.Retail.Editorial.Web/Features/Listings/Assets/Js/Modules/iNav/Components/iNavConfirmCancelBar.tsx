import React from 'react'
import { connect } from 'react-redux'
import { Dispatch } from 'redux'
import { Actions, ActionTypes, Thunks } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'


if (!SERVER) {
    require('iNav/Css/iNav.ConfirmCancelBar')  
  }

//TODO: how to get test data driven?

interface IINavConfirmCancelBar {
    count: number
    pendingQuery: string
}

const INavConfirmCancelBar: React.StatelessComponent<IINavConfirmCancelBar> = ({count, onClick, pendingQuery}) => (
    <div className='iNav-category__confirmCancelBar confirmCancelBar'>
        <div className='confirmCancelBar__button confirmCancelBar__button--cancel'>Cancel</div>
        <div className='confirmCancelBar__button confirmCancelBar__button--show' onClick={()=>onClick(pendingQuery)}>Show {count} Articles</div>
    </div>
)

const mapStateToProps = (state: any) => {
    return {
        count: state.iNav.count,
        pendingQuery: state.iNav.pendingQuery ? state.iNav.pendingQuery : '' 
    }
}

const mapDispatchToProps = (dispatch: Dispatch<Thunks.fetchINav>) => {
    return {
        onClick: (query: string)=>dispatch([
            Thunks.fetchINav(query),
            {type:ActionTypes.UI.CLOSE_INAV}
        ])
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(INavConfirmCancelBar)