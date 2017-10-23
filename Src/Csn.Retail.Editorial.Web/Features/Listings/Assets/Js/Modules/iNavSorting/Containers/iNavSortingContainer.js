import React from 'react'
import { connect } from 'react-redux'
import { Thunks } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'

if (!SERVER) {
    require('iNavSorting/Css/iNavSorting.scss')
}

const INavSorOption = ({ selected, label, value,query }) => {
        return <option value={value} selected={selected}>{label}</option>
    // } else {
    //     //need to fix the on change event // skip should be 0 when selected
    //     //<option selected="true"><a href="{iNav.home(!!query? query:'', limit, 0, value)}"> {label}</a></option>
    //     return <option value={value}>{label}</option>
    // }
}

const INavSorting = ({ sorting, limit, query, isVisible }) =>  {    
     return <div className={`iNavSorting__container ${isVisible ? '' : 'hide' }`}>
            <select className='iNavSorting' onChange={()=>{fetchQuery({q:query})}}>
                {sorting.sortListItems.map((sortItem) =>{
                    return  <INavSorOption key={sortItem.value} { ...sortItem } ></INavSorOption>
                })}             
            </select>
        </div>
}

// Redux Connect
const mapStateToProps = (state) => {
    return {
        isVisible: !!state.store.listings.navResults.count,
        sorting: state.store.listings.sorting,
        query: state.store.listings.currentQuery
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        fetchQuery: (query) => {dispatch([
            Thunks.fetchINav(query)
        ])}
    }
}


// Connect the Component to the store
const INavSortingContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(INavSorting)

export default INavSortingContainer
