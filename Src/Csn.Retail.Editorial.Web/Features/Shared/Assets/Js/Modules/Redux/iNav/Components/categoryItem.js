import React from 'react'

const CategoryItem = ({ isSelected, displayValue, name, toggleSelected}) =>
    (
    <div>
        <input 
            type="checkbox" 
            checked={isSelected}
            onChange={() => { toggleSelected(isSelected, name, displayValue) }}/>
            {displayValue}
    </div>
    )




export default CategoryItem