import React from 'react'
import { connect } from 'react-redux'


if (!SERVER) {
    require('iNav/Css/iNav.ConfirmCancelBar')  
  }

//TODO: how to get test data driven?

interface IINavConfirmCancelBar {
    count: number
}

const INavConfirmCancelBar: React.StatelessComponent<IINavConfirmCancelBar> = ({count}) => (
    <div className='iNav-category__confirmCancelBar confirmCancelBar'>
        <div className='confirmCancelBar__button confirmCancelBar__button--cancel'>Cancel</div>
        <div className='confirmCancelBar__button confirmCancelBar__button--show'>Show {count} Articles</div>
    </div>
)

const mapStateToProps = (state: any) => {
    return {
        count: state.iNav.count
    }
}

export default connect(
    mapStateToProps
)(INavConfirmCancelBar)