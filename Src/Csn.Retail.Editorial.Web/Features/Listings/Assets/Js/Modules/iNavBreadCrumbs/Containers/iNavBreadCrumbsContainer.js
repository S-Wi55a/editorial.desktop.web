import React from 'react'
import { connect } from 'react-redux'
import { CSSTransitionGroup } from 'react-transition-group'
import { Thunks } from 'iNav/Actions/actions'

// TODO: use enum in TS for this
const FacetBreadCrumb = {
    keyword: 'KeywordBreadCrumb',
    facet: 'FacetBreadCrumb',
    clearAll: 'ClearAllBreadCrumb'
}

const INavBreadCrumb = ({facetDisplay, removeAction, fetchINav, type}) => {

    if(type === FacetBreadCrumb.clearAll || type === FacetBreadCrumb.keyword){
        return <a className={`iNavBreadCrumb iNavBreadCrumb--${type}`} href={removeAction} onClick={(e)=>{e.preventDefault(); fetchINav(removeAction, true);}}>{type === FacetBreadCrumb.keyword ? 'Keywords: ':''}{facetDisplay}</a>
    }
    return <a className={`iNavBreadCrumb iNavBreadCrumb--${type}`} href={removeAction} onClick={(e)=>{e.preventDefault(); fetchINav(removeAction);}}>{type === FacetBreadCrumb.keyword ? 'Keywords: ':''}{facetDisplay}</a>    
    
}

const INavBreadCrumbs = ({ breadCrumbs, fetchINav }) => (
        <div className="iNavBreadCrumbs">
            <CSSTransitionGroup
                transitionName="iNavBreadCrumbTransition"
                transitionEnterTimeout={300}
                transitionLeaveTimeout={300}>
                {breadCrumbs.map((breadCrumb) => {
                    return <INavBreadCrumb key={breadCrumb.facetDisplay} {...breadCrumb} fetchINav={fetchINav}/>
                })}
            </CSSTransitionGroup>
        </div>
    )

// Redux Connect
const mapStateToProps = (state) => {
    return {
        breadCrumbs: state.store.listings.navResults.iNav.breadCrumbs ? state.store.listings.navResults.iNav.breadCrumbs : []
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        fetchINav: (query, clear)=>dispatch(Thunks.fetchINav(query, clear))
    }
  }

// Connect the Component to the store
const INavBreadCrumbsContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(INavBreadCrumbs)

export default INavBreadCrumbsContainer

