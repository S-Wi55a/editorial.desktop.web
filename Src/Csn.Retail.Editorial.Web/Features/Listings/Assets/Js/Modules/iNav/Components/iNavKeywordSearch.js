import React from 'react'
import { connect } from 'react-redux'
import { Thunks } from 'iNav/Actions/actions'
import { Field, reduxForm, change } from 'redux-form'

if (!SERVER) {
  require('iNav/Css/iNav.keywordSearch')
}

let KeywordSearchComponent = (props) => {

  return (
    <div className={`iNav__keywordSearch`} data-webm-section="keyword">
      <div className={`iNav__keywordSearch-container ${props.keywordSearchIsActive ? 'iNav__keywordSearch-container--isActive' : ''}`} >
        {/* When in focus update class*/}
        <form onSubmit={(e)=>{e.preventDefault();props.fetchSearchResults(props.currentQuery)}} className={`iNav__keywordSearch-form`} data-webm-clickvalue={`input`}>
          <Field 
            name="keyword" 
            component="input" 
            type="text"
            className={`iNav__keywordSearch-input`} 
            placeholder={'Keyword search'}
            autoComplete="on"            
          />
        </form>
        {/* button needs to hav click which triggers keyword api call*/}
        <button 
          className="iNav__keywordSearch-button iNav__keywordSearch-button--search" 
          type="submit" 
          onClick={(e)=>{e.preventDefault();props.fetchSearchResults(props.currentQuery)}}
          data-webm-clickvalue={`search`}
        ></button>
        <button 
          className="iNav__keywordSearch-button iNav__keywordSearch-button--clear" 
          onClick={props.clear}
          data-webm-clickvalue={`clear`}
        ></button>
      </div>
    </div>
  )
}

const mapStateToProps = (state) => {
  return {
    currentQuery: state.store.listings.currentQuery,
    initialValues: {
      keyword: state.store.listings.keyword
    }
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
      fetchSearchResults: (query)=>{
        dispatch(Thunks.fetchINav(query))
      },
      clear: ()=>dispatch(change('keywordSearch', 'keyword', ''))
  }
}

const KeywordSearch = connect(
  mapStateToProps,
  mapDispatchToProps
)(reduxForm({
  // a unique name for the form
  form: 'keywordSearch',
  enableReinitialize: true
})(KeywordSearchComponent))

export default KeywordSearch;