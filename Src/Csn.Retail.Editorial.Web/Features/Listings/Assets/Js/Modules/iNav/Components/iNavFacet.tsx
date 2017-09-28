﻿import React from 'react'
import { IFacet } from 'iNav/Types'
import { iNav } from 'Endpoints/endpoints'
import { connect } from 'react-redux'
import { Dispatch } from 'redux';
import { Actions, ActionTypes, Thunks } from 'iNav/Actions/actions'

if (!SERVER) {
    require('iNav/Css/iNavListItem')  
  }


interface IINavfacet extends IFacet {
    aspect: string
    onClick: Thunks.Types
}

const INavfacet: React.StatelessComponent<IINavfacet> = ({ isSelected, displayValue, value, action, count, aspect, isRefineable, onClick }) => {
    
    return (
        <li className={`iNav-category-item ${isSelected ? 'isSelected': ''} ${count ? '' : 'iNav-category-item--noResults' }`} onClick={()=>{count > 0 && onClick(aspect, action)}} >
            <input className="iNav-category-item__checkbox" type="checkbox" checked={isSelected} readOnly={true}/>
            <a className="iNav-category-item__link" href={`${iNav}${action}`} onClick={e => e.preventDefault()}>{displayValue}</a>
            <span className="iNav-category-item__meta-container">
                <span className="iNav-category-item__count">{count}</span>
                {                
                    isRefineable ? <span className="iNav-category-item__refinement"></span> : ''
                }
            </span>

        </li>
    )
}


const mapDispatchToProps = (dispatch: any) => {
    return {
        onClick: (aspect:string, query: string)=>dispatch([
            { type: ActionTypes.INAV.UPDATE_PENDING_QUERY, payload: {query} },
            Thunks.fetchINavAspect(aspect, query)
        ])
    }
}

export default connect(
    null,
    mapDispatchToProps
)(INavfacet)