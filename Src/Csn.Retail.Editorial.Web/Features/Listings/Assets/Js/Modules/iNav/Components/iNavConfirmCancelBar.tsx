import React from 'react'
import { connect } from 'react-redux'
import { Dispatch } from 'redux'
import { Actions, ActionTypes, Thunks } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'
import { State } from 'iNav/Types'



if (!SERVER) {
    require('iNav/Css/iNav.ConfirmCancelBar')  
  }

//TODO: how to get test data driven?

interface IINavConfirmCancelBar {
    index?: number
    count: number
    pendingQuery: string
}

const INavConfirmCancelBar: React.StatelessComponent<IINavConfirmCancelBar> = ({count, onClick, pendingQuery}) => (
    <div className='iNav-category__confirmCancelBar confirmCancelBar'>
        <div className='confirmCancelBar__button confirmCancelBar__button--cancel'>Cancel</div>
        <div className='confirmCancelBar__button confirmCancelBar__button--show' onClick={()=>onClick(pendingQuery)}>Show {count} Articles</div>
    </div>
)

const mapStateToProps = (state: State, ownProps: IINavConfirmCancelBar) => {
    return {
        count: typeof state.iNav.iNav.nodes[ownProps.index].count !== 'undefined' ? state.iNav.iNav.nodes[ownProps.index].count : state.iNav.count,
        pendingQuery: state.iNav.pendingQuery ? state.iNav.pendingQuery : '' 
    }
}

const mapDispatchToProps = (dispatch: Dispatch<any>) => {
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