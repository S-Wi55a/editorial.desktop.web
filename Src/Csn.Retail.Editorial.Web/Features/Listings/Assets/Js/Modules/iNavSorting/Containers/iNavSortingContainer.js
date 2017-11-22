import React from 'react'
import { connect } from 'react-redux'
import { Thunks } from 'iNav/Actions/actions'
import { Field, reduxForm } from 'redux-form'

if (!SERVER) {
    require('iNavSorting/Css/iNavSorting.scss')
}

const INavSorting = ({ sorting, isVisible, fetchQuery }) =>  {    
     return <div className={`iNavSorting__container ${isVisible ? '' : 'hide' }`}>
                <Field name="sortOrder" component="select" className='iNavSorting' onChange={(event, newValue)=>fetchQuery(event, newValue)}>
                    {sorting.sortListItems.map((sortItem) => <option key={sortItem.value} value={sortItem.url}>{sortItem.label}</option>)}   
                </Field>            
            </div>
}

// Redux Connect
const mapStateToProps = (state) => {
    return {
        isVisible: !!state.store.listings.navResults.count,
        sorting: state.store.listings.sorting,
        initialValues: {
            sortOrder: state.store.listings.sorting.sortListItems.find(el=>el.selected === true).url
          }
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        fetchQuery: (event, newValue) => {
            dispatch(Thunks.fetchINavAndResults(newValue));
        }
    }
}


const INavSortingContainer = connect(
    mapStateToProps,
    mapDispatchToProps
  )(reduxForm({
    // a unique name for the form
    form: 'sort',
    enableReinitialize: true
  })(INavSorting))

export default INavSortingContainer
