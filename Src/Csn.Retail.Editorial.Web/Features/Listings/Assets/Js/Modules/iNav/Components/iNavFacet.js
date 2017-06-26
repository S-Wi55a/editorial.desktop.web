import React from 'react'
import { ryvuss } from 'Js/Modules/Endpoints/endpoints'

const INavfacet = ({ isSelected, displayValue, name, toggleIsSelected, action, count }) => {
    

    //console.log('Category Item')


    return (
        <li className={`iNav-category-item ${isSelected ? 'isSelected': ''}`} onClick={() => {toggleIsSelected(isSelected, name, displayValue, action)}}>
            <input className="iNav-category-item__checkbox" type="checkbox" checked={isSelected} readOnly="true"/>
            <a className="iNav-category-item__link" href={`${ryvuss.iNavWithCount}${action}`} onClick={e => e.preventDefault()}>{displayValue}</a>
            <span className="iNav-category-item__count-container">
                <span className="iNav-category-item__count">{count}</span>
            </span>
        </li>
    )

}
export default INavfacet