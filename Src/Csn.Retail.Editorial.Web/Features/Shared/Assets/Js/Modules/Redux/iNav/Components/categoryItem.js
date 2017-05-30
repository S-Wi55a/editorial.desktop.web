import React from 'react'
import { ryvuss } from 'Js/Modules/Endpoints/endpoints'

const CategoryItem = ({ isSelected, displayValue, name, toggleSelected, action, count }) => {
    
    
    return (
        <div className={`searchbar-category-item ${isSelected ? 'isSelected': ''}`} onClick={() => {toggleSelected(isSelected, name, displayValue)}}>
            <input className="searchbar-category-item__checkbox" type="checkbox" checked={isSelected} readOnly="true"/>
            <a className="searchbar-category-item__link" href={`${ryvuss.iNavWithCount}${action}`} onClick={e => e.preventDefault()}>{displayValue}</a>
            <span className="searchbar-category-item__count-container">
                <span className="searchbar-category-item__count">{count}</span>
            </span>
        </div>
    )

}
export default CategoryItem