import React from 'react'
import { } from 'iNav/Types'
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
}


//TODO: Should add Keywors Search placeholder text to language file
const KeywordInput: React.StatelessComponent<any> = (props) => (
    <input {...props.input} 
      type="text" 
      className={`iNav__keywordSearch-input ${props.meta.active ? 'iNav__keywordSearch-input--isActive' : ''}`} 
      placeholder={'Keyword search'}
    />
)

let KeywordSearchComponent: React.StatelessComponent<IKeywordComponent> = (props) => {

  return (
    <div className={`iNav__keywordSearch`}>
      <form className={`iNav__keywordSearch-container ${props.keywordSearchIsActive ? 'iNav__keywordSearch-container--isActive' : ''}`} onSubmit={ props.handleSubmit }>
        {/* When in focus update class*/}
        <Field name="keyword" component={KeywordInput} type="text"/>
        {/* button needs to hav click which triggers keyword api call*/}
        <button className="iNav__keywordSearch-button iNav__keywordSearch-button--search" type="submit"></button>
        <button className="iNav__keywordSearch-button iNav__keywordSearch-button--clear" onClick={()=>{/* clear text */}}></button>
      </form>
    </div>
  )
}


const KeywordSearch: any = reduxForm({
  // a unique name for the form
  form: 'keywordSearch'
})(KeywordSearchComponent)

export default KeywordSearch;