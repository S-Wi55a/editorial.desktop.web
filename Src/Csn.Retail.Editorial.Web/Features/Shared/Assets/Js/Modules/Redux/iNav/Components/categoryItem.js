import React from 'react'
import { ryvuss } from 'Js/Modules/Endpoints/endpoints'

const CategoryItem = ({ isSelected, displayValue, name, toggleSelected, action, count }) => {
    
    return (
        <div onClick={() => {toggleSelected(isSelected, name, displayValue)}}>
        <input type="checkbox" checked={isSelected} readOnly="true"/>
        <a href={`${ryvuss.iNavWithCount}${action}`} onClick={e => e.preventDefault()}>{displayValue}</a>{count}

    </div>
    )

}
export default CategoryItem