﻿import React from 'react'
import { State, IFacet, IRefinement } from 'iNav/Types'
import { iNav } from 'Endpoints/endpoints'
import { connect } from 'react-redux'
import { Actions, ActionTypes, Thunks } from 'iNav/Actions/actions'

if (!SERVER) {
  require('iNav/Css/iNavListItem')
}

interface IINavfacet extends IFacet {
  aspect: string;
  id: number;
  refinement?: IRefinement;
  fetchINavAndUpdatePendingRequest?: Thunks.Types;
  fetchINavAndResults?: Thunks.Types;
  fetchAspect?: Thunks.Types;
  fetchRefinementAndUpdatePendingRequest?: Thunks.Types;
  fetchRefinementAndSwitchPage?: (aspect: string, refinementAspect: string, refinementParentExpression: string, pendingQuery: string, refinementId: number)=>Actions;
  pendingQuery?: string;
  pendingAction?: string
  isRefinement?: boolean;
}

const Refinements: React.StatelessComponent<any> = (props) => (
  <div className="iNav-category-item__refinements--hidden">
    {
      props.isSelected && props.refinements ? props.refinements.facets.map((node: any, index: any) => {
        return <a href={`${node.url}`} key={`${index}`}>{node.displayValue}</a>
    }) : ''
    }
  </div>
)

const INavfacet: React.StatelessComponent<IINavfacet> = (props) => {
  return (
    <li className={`iNav-category-item ${props.isSelected ? 'isSelected' : ''} ${props.count ? '' : 'iNav-category-item--noResults'}`}
    data-webm-clickvalue={props.displayValue}
    onClick={
      () => {
        if (props.isRefinement) {
            props.count > 0 &&
                props.fetchRefinementAndUpdatePendingRequest(
                    props.aspect,
                    props.refinement.aspect,
                    props.refinement.parentExpression,
                    props.action,
                    props.url
                );
        }
        else {
            props.count > 0 && props.fetchINavAndUpdatePendingRequest(props.action, props.url);
        }
      }
    } >
      <input className="iNav-category-item__checkbox" type="checkbox" checked={props.isSelected} readOnly={true} />
      {
        props.count === 0 ?
        <div className="iNav-category-item__link">{props.displayValue}</div>
        :
        <a className="iNav-category-item__link" href={`${props.url}`} onClick={e => e.preventDefault()}>{props.displayValue}</a>        
      }
      

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
                props.pendingAction,
                props.id
              )
            }
          }
          data-webm-clickvalue={`refinement`}
          ></span> : ''
        }
      </span>
      <Refinements {...props} />
      </li>

  )
}

const mapStateToProps = (state: any) => {
  return {
      pendingAction: typeof state.store.listings.navResults.iNav.pendingAction !== 'undefined' ? state.store.listings.navResults.iNav.pendingAction : state.store.listings.navResults.iNav.currentAction,
  }
}

const mapDispatchToProps: any = (dispatch: any, ownProps: IINavfacet) => {
    return {
        fetchINavAndUpdatePendingRequest: (action: string, url: string) =>
            dispatch([
                { type: ActionTypes.INAV.UPDATE_PENDING_ACTION, payload: { url, action } },
                Thunks.fetchINav(action)
            ]),
        fetchRefinementAndUpdatePendingRequest: (aspect: string, refinementAspect: string, refinementParentExpression: string, action: string, url: string) => 
            dispatch([
                { type: ActionTypes.INAV.UPDATE_PENDING_ACTION, payload: { url, action } },
                Thunks.fetchINavRefinement(aspect, refinementAspect, refinementParentExpression, action)
            ]),
        fetchRefinementAndSwitchPage: (aspect: string, refinementAspect: string, refinementParentExpression: string, action: string, refinementId: number) =>
            dispatch([
                Thunks.fetchINavRefinement(aspect, refinementAspect, refinementParentExpression, action, null, 
                { type: ActionTypes.UI.SWITCH_PAGE_FORWARD, payload: { refinementId } })
            ])
    }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(INavfacet)