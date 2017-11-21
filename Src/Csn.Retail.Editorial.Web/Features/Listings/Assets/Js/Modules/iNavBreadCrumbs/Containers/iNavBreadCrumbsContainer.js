import React from 'react'
import { connect } from 'react-redux'
import { FadeIn } from 'ReactAnimations/Fade'
import Timer from 'ReactAnimations/Timer'
import { Thunks } from 'iNav/Actions/actions'

// TODO: use enum in TS for this
const FacetBreadCrumb = {
    keyword: 'KeywordBreadCrumb',
    facet: 'FacetBreadCrumb',
    clearAll: 'ClearAllBreadCrumb'
}

const INavBreadCrumb = ({ facetDisplay, removeAction, fetchINavAndResults, type}) => {

    if(type === FacetBreadCrumb.clearAll || type === FacetBreadCrumb.keyword){
        return <a className={`iNavBreadCrumb iNavBreadCrumb--${type}`} href={removeAction} onClick={(e)=>{e.preventDefault(); fetchINavAndResults(removeAction, true);}}>{type === FacetBreadCrumb.keyword ? 'Keywords: ':''}{facetDisplay}</a>
    }
    return <a className={`iNavBreadCrumb iNavBreadCrumb--${type}`} href={removeAction} onClick={(e)=>{e.preventDefault(); fetchINavAndResults(removeAction);}}>{type === FacetBreadCrumb.keyword ? 'Keywords: ':''}{facetDisplay}</a>    
    
}

const INavBreadCrumbs = ({ breadCrumbs, fetchINavAndResults }) => (
        <div className="iNavBreadCrumbs">
            {breadCrumbs.map((breadCrumb) => {
                return  <Timer key={`${breadCrumb.facetDisplay}${Math.random()}`}>
                            <FadeIn duration={300} startingOpacity={0} className="d-inline-block">
                        <INavBreadCrumb  {...breadCrumb} fetchINavAndResults={fetchINavAndResults}/>
                            </FadeIn>
                        </Timer>
            })}
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
        fetchINavAndResults: (query, clear) => dispatch(Thunks.fetchINavAndResults(query, clear))
    }
  }

// Connect the Component to the store
const INavBreadCrumbsContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(INavBreadCrumbs)

export default INavBreadCrumbsContainer

