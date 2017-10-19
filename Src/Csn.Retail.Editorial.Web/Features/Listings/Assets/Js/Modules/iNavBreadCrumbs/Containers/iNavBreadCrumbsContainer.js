import React from 'react'
import { connect } from 'react-redux'
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { Thunks } from 'iNav/Actions/actions'

const INavBreadCrumb = ({facetDisplay, removeAction, fetchINav}) => (
    <a className="iNavBreadCrumb" href={`?q=${removeAction}`} onClick={(e)=>{e.preventDefault(); fetchINav(removeAction);}}>{facetDisplay}</a>    
)

const INavBreadCrumbs = ({ breadCrumbs, fetchINav }) => (
        <div className="iNavBreadCrumbs">
            <ReactCSSTransitionGroup
                transitionName="iNavBreadCrumbTransition"
                transitionEnterTimeout={300}
                transitionLeaveTimeout={300}>
                {breadCrumbs.map((breadCrumb) => {
                    return <INavBreadCrumb key={breadCrumb.facetDisplay} {...breadCrumb} fetchINav={fetchINav}/>
                })}
            </ReactCSSTransitionGroup>
        </div>
    )

// Redux Connect
const mapStateToProps = (state) => {
    return {
        breadCrumbs: state.iNav.iNav.breadCrumbs ? state.iNav.iNav.breadCrumbs : []
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        fetchINav: (query)=>dispatch(Thunks.fetchINav(query))
    }
  }

// Connect the Component to the store
const INavBreadCrumbsContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(INavBreadCrumbs)

export default INavBreadCrumbsContainer

