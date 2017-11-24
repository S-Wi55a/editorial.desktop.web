import React from 'react'
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
  fetchINavAndUpdatePendingQuery?: Thunks.Types;
  fetchINavAndResults?: Thunks.Types;
  fetchAspect?: Thunks.Types;
  fetchRefinementAndUpdatePendingQuery?: Thunks.Types;
  fetchRefinementAndSwitchPage?: (aspect: string, refinementAspect: string, refinementParentExpression: string, pendingQuery: string, refinementId: number)=>Actions;
  pendingQuery?: string;
  isRefinement?: boolean;
}

const INavfacet: React.StatelessComponent<IINavfacet> = (props) => {
  return (
    <li className={`iNav-category-item ${props.isSelected ? 'isSelected' : ''} ${props.count ? '' : 'iNav-category-item--noResults'}`}
    data-webm-clickvalue={props.displayValue}
    onClick={
      () => {
        if (props.isRefinement) {
            props.count > 0 &&
                props.fetchRefinementAndUpdatePendingQuery(
                    props.aspect,
                    props.refinement.aspect,
                    props.refinement.parentExpression,
                    props.action,
                    props.url
                );
        }
        else {
            props.count > 0 && props.fetchINavAndUpdatePendingQuery(props.action, props.url);
        }
      }
    } >
      <input className="iNav-category-item__checkbox" type="checkbox" checked={props.isSelected} readOnly={true} />
      <a className="iNav-category-item__link" href={`${props.url}`} onClick={e => e.preventDefault()}>{props.displayValue}</a>
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
                props.action,
                props.id
              )
            }
          }
          data-webm-clickvalue={`refinement`}
          ></span> : ''
        }
      </span>
      {props.isSelected ? props.refinements.facets.map((node, index) => {
              return <a className="iNav-category-item__hiddenlink" href={`${node.url}`} key={`${index}`}>
                  {node.displayValue}
              </a>
      }) : ''}
      </li>

  )
}

const mapDispatchToProps: any = (dispatch: any, ownProps: IINavfacet) => {
    return {
        fetchINavAndUpdatePendingQuery: (query: string, url: string) =>
            dispatch([
                { type: ActionTypes.INAV.UPDATE_PENDING_QUERY, payload: { query: url } },
                Thunks.fetchINav(query)
            ]),
        fetchRefinementAndUpdatePendingQuery: (aspect: string, refinementAspect: string, refinementParentExpression: string, pendingQuery: string, url: string) => 
            dispatch([
                { type: ActionTypes.INAV.UPDATE_PENDING_QUERY, payload: { query: url } },        
                Thunks.fetchINavRefinement(aspect, refinementAspect, refinementParentExpression, pendingQuery)
            ]),
        fetchRefinementAndSwitchPage: (aspect: string, refinementAspect: string, refinementParentExpression: string, pendingQuery: string, refinementId: number) =>
            dispatch([
                Thunks.fetchINavRefinement(aspect, refinementAspect, refinementParentExpression, pendingQuery, null, 
                { type: ActionTypes.UI.SWITCH_PAGE_FORWARD, payload: { refinementId } })
            ])
    }
}

export default connect(
  null,
  mapDispatchToProps
)(INavfacet)