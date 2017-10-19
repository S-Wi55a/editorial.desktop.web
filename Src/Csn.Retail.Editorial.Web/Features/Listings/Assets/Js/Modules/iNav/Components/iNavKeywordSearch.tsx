import React from 'react'
import { State } from 'iNav/Types'
import { iNav } from 'Endpoints/endpoints'
import { connect } from 'react-redux'
import { Actions, ActionTypes, Thunks } from 'iNav/Actions/actions'
import { Field, reduxForm, InjectedFormProps } from 'redux-form'


if (!SERVER) {
  require('iNav/Css/iNav.keywordSearch')
}

interface IKeywordComponent extends InjectedFormProps {
  placeholderText: string
  keywordSearchIsActive: boolean
  fetchSearchResults?: (q: string)=>void
  pendingQuery?: string
  pendingKeywordValue?: string
}


//TODO: Should add Keywors Search placeholder text to language file
const KeywordInput: React.StatelessComponent<any> = (props) => (
    <input {...props.input} 
      type="text" 
      className={`iNav__keywordSearch-input ${props.meta.active ? 'iNav__keywordSearch-input--isActive' : ''}`} 
      placeholder={'Keyword search'}
      autoComplete="on"
    />
)

let KeywordSearchComponent: React.StatelessComponent<any> = (props) => {

  return (
    <div className={`iNav__keywordSearch`}>
      <form className={`iNav__keywordSearch-container ${props.keywordSearchIsActive ? 'iNav__keywordSearch-container--isActive' : ''}`} onSubmit={ props.handleSubmit }>
        {/* When in focus update class*/}
        <Field name="keyword" component={KeywordInput} type="text"/>
        {/* button needs to hav click which triggers keyword api call*/}
        <button className="iNav__keywordSearch-button iNav__keywordSearch-button--search" type="submit" onClick={props.fetchSearchResults}></button>
        <button className="iNav__keywordSearch-button iNav__keywordSearch-button--clear"></button>
      </form>
    </div>
  )
}

const mapStateToProps = (state: any, ownProps: IKeywordComponent) => {
  return {
      pendingQuery: state.iNav.pendingQuery ? state.iNav.pendingQuery : '',
      pendingKeywordValue: state.form && state.form.keywordSearch && state.form.keywordSearch.registeredFields.keyword && state.form.keywordSearch.values && state.form.keywordSearch.values.keyword ? `&keyword=${state.form.keywordSearch.values.keyword}` : '' 
  }
}

const mapDispatchToProps = (dispatch: any) => {
  return {
      fetchSearchResults: ()=>dispatch(Thunks.fetchINav()),
      cancel: ()=>{
          dispatch({type: ActionTypes.UI.CANCEL})
        }
  }
}

const KeywordSearch: any = reduxForm({
  // a unique name for the form
  form: 'keywordSearch'
})(connect(
  mapStateToProps,
  mapDispatchToProps
)(KeywordSearchComponent))

export default KeywordSearch;