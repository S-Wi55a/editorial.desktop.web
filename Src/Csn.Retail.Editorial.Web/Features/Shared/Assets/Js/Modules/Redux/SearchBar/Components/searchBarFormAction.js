import React from 'react'

const SearchBarFormAction = ({resetForm, ryvuss, count, href}) => {

    //TODO: remove hard coded text
    return (
        <div className="searchbar-form-action">
            <button className="searchbar-form-action__button searchbar-form-action__button--clear" onClick={resetForm}>Clear</button>
            <a className="searchbar-form-action__button searchbar-form-action__button--confirm" href={href}>{count} Articles</a>
        </div>
    )
}

export default SearchBarFormAction