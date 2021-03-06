﻿import React from 'react'
import { connect } from 'react-redux'
import { FadeIn } from 'ReactAnimations/Fade'
import Timer from 'ReactAnimations/Timer'
import { Thunks } from 'ReactComponents/iNav/Actions/actions'

// TODO: use enum in TS for this
const FacetBreadCrumb = {
    keyword: 'KeywordBreadCrumb',
    facet: 'FacetBreadCrumb',
    clearAll: 'ClearAllBreadCrumb'
}

const INavBreadCrumb = ({ facetDisplay, removeAction, fetchINavAndResults, type}) => {
    return <a 
        className={`iNavBreadCrumb iNavBreadCrumb--${type}`} 
        href={removeAction} 
        onClick={(e)=>{e.preventDefault(); fetchINavAndResults(removeAction);}}
        data-webm-clickvalue={facetDisplay}
        >
            {facetDisplay}
        </a>
}

const INavBreadCrumbs = ({ breadCrumbs, fetchINavAndResults }) => (
        <div className="iNavBreadCrumbs" data-webm-section="tags">
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
        breadCrumbs: state.store.nav.navResults.iNav.breadCrumbs ? state.store.nav.navResults.iNav.breadCrumbs : []
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        fetchINavAndResults: (query) => dispatch(Thunks.fetchINavAndResults(query))
    }
  }

// Connect the Component to the store
const INavBreadCrumbsContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(INavBreadCrumbs)

export default INavBreadCrumbsContainer

