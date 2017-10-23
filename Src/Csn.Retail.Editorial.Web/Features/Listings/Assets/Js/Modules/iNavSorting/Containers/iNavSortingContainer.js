import React from 'react'
import { connect } from 'react-redux'
import { Thunks } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'

if (!SERVER) {
    require('iNavSorting/Css/iNavSorting.scss')
}

const INavSorOption = ({ selected, label, value, query }) => {
    if (selected) {
        return <option value={value} selected='true'>{label}</option>
    } else {
        //need to fix the on change event // skip should be 0 when selected
        //<option selected="true"><a href="{iNav.home(!!query? query:'', 0, value)}"> {label}</a></option>
        return <option value={value}>{label}</option>
    }
}

const INavSorting = ({ sorting }) =>  {
     return <div className='iNavSorting__container'>
            <select className='iNavSorting iNavSorting--select' >
                {sorting.sortListItems.map((sortItem) =>{
                    return  <INavSorOption key={sortItem.value} { ...sortItem } ></INavSorOption>
                })}             
            </select>
        </div>
}

// Redux Connect
const mapStateToProps = (state) => {
    return {
        sorting: state.iNav.sorting
    }
}

// Connect the Component to the store
const INavSortingContainer = connect(
    mapStateToProps
)(INavSorting)

export default INavSortingContainer