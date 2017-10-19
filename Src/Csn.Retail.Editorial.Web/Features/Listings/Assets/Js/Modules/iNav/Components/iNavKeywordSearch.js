import React from 'react'
import { connect } from 'react-redux'
import { Thunks } from 'iNav/Actions/actions'
import { Field, reduxForm, change } from 'redux-form'

if (!SERVER) {
  require('iNav/Css/iNav.keywordSearch')
}

let KeywordSearchComponent = (props) => {

  return (
    <div className={`iNav__keywordSearch`}>
      <div className={`iNav__keywordSearch-container ${props.keywordSearchIsActive ? 'iNav__keywordSearch-container--isActive' : ''}`} >
        {/* When in focus update class*/}
        <Field 
          name="keyword" 
          component="input" 
          type="text"
          className={`iNav__keywordSearch-input`} 
          placeholder={'Keyword search'}
          autoComplete="on"
        />
        {/* button needs to hav click which triggers keyword api call*/}
        <button className="iNav__keywordSearch-button iNav__keywordSearch-button--search" type="submit" onClick={props.fetchSearchResults}></button>
        <button className="iNav__keywordSearch-button iNav__keywordSearch-button--clear" onClick={props.clear}></button>
      </div>
    </div>
  )
}

const mapStateToProps = (state) => {
  return {
    initialValues: {
      keyword: state.iNav.keyword
    }
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
      fetchSearchResults: ()=>dispatch(Thunks.fetchINav()),
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