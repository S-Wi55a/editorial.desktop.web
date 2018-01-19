import React from 'react'
import { connect } from 'react-redux'
import { Thunks } from 'ReactComponents/iNav/Actions/actions'
import { Field, reduxForm } from 'redux-form'
import { getParameterByName } from 'Parse/parse'

if (!SERVER) {
    require('iNavSorting/Css/iNavSorting.scss')
}

const INavSorting = ({ sorting, isVisible, fetchQuery }) =>  {    
     return <div className={`iNavSorting__container ${isVisible ? '' : 'hide' }`} data-webm-section={`sort`}>
                <Field name="sortOrder" component="select" className='iNavSorting' 
                    onChange={(event, newValue)=>{
                        const sortValue = getParameterByName('sort', newValue)
                        typeof CsnInsightsEventTracker !== 'undefined' ? CsnInsightsEventTracker.sendClick(undefined,'sort', sortValue, undefined,undefined) : false
                        fetchQuery(event, newValue)
                    }}>
                    {sorting.sortListItems.map((sortItem) => <option key={sortItem.value} value={sortItem.url}>{sortItem.label}</option>)}   
                </Field>            
            </div>
}

// Redux Connect
const mapStateToProps = (state) => {
    return {
        isVisible: !!state.store.nav.navResults.count,
        sorting: state.store.nav.sorting,
        initialValues: {
            sortOrder: state.store.nav.sorting.sortListItems.find(el=>el.selected === true).url
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
