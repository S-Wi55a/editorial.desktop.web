import React from 'react'
import { State, IFacet, IRefinement } from 'iNav/Types'
import { iNav } from 'Endpoints/endpoints'
import { connect } from 'react-redux'
import { Actions, ActionTypes, Thunks } from 'iNav/Actions/actions'

if (!SERVER) {
  require('iNav/Css/iNavListItem')
}

interface IINavfacet extends IFacet {
  aspect: string  
  id: number  
  refinement?: IRefinement
  fetchAspect?: Thunks.Types
  fetchRefinementAndUpdatePendingQuery?: Thunks.Types
  fetchRefinementAndSwitchPage?: (aspect: string, refinementAspect: string, refinementParentExpression: string, pendingQuery: string, refinementId: number)=>Actions  
  pendingQuery?: string
  isRefinement?: boolean
}

const INavfacet: React.StatelessComponent<IINavfacet> = (props) => {
  return (
    <li className={`iNav-category-item ${props.isSelected ? 'isSelected' : ''} ${props.count ? '' : 'iNav-category-item--noResults'}`}
    data-webm-clickvalue={props.displayValue}
    onClick={
      () => {
        if (props.isRefinement) {
          props.count > 0 && props.fetchRefinementAndUpdatePendingQuery(
            props.aspect,
            props.refinement.aspect,
            props.refinement.parentExpression,
            props.action
          )
        }
        else {
          props.count > 0 && props.fetchAspect(props.aspect, props.action)
        }
      }
    } >
      <input className="iNav-category-item__checkbox" type="checkbox" checked={props.isSelected} readOnly={true} />
      <a className="iNav-category-item__link" href={`${props.action}`} onClick={e => e.preventDefault()}>{props.displayValue}</a>
      <span className="iNav-category-item__meta-container">
        <span className="iNav-category-item__count">{props.count}</span>
        {
          props.isRefineable ? <span className="iNav-category-item__refinement" onClick={
            (e) => {
              e.stopPropagation()
              props.fetchRefinementAndSwitchPage(
                props.aspect,
                props.refinement.aspect,
                props.refinement.parentExpression,
                props.pendingQuery,
                props.id
              )
            }
          }
          data-webm-clickvalue={`refinement`}
          ></span> : ''
        }
      </span>
    </li>
  )
}

const mapDispatchToProps: any = (dispatch: any, ownProps: IINavfacet) => {
  return {
    fetchAspect: (aspect: string, query: string) => dispatch([
      { type: ActionTypes.INAV.UPDATE_PENDING_QUERY, payload: { query } },
      Thunks.fetchINavAspect(aspect, query)
    ]),
    fetchRefinementAndUpdatePendingQuery: (aspect: string, refinementAspect: string, refinementParentExpression: string, pendingQuery: string) => {
      return dispatch([
        { type: ActionTypes.INAV.UPDATE_PENDING_QUERY, payload: { query: pendingQuery } },        
        Thunks.fetchINavRefinement(aspect, refinementAspect, refinementParentExpression, pendingQuery)
      ])
    },
    fetchRefinementAndSwitchPage: (aspect: string, refinementAspect: string, refinementParentExpression: string, pendingQuery: string, refinementId: number) => {
      return dispatch([
        Thunks.fetchINavRefinement(aspect, refinementAspect, refinementParentExpression, pendingQuery, { type: ActionTypes.UI.SWITCH_PAGE_FORWARD, payload: {refinementId}}),
      ])
    }
  }
}

export default connect(
  null,
  mapDispatchToProps
)(INavfacet)