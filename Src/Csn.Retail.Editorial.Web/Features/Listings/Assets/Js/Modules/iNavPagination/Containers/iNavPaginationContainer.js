import React from 'react'
import { connect } from 'react-redux'
import { Thunks } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'

if (!SERVER) {
    require('iNavPagination/Css/iNavPagination.scss')  
}

const INavPageNavigator = ({ text, limit, query, show, skip }) => {    
    if(!!show){
        return <a className='iNavPageItem' href={iNav.home(!!query? query:'', limit, skip)}>
            {text}
        </a>
    }
    return null;
}
const INavPageSeparator = ({ show }) => {    
    if (show) {
        return <div className='iNavPageSeparator'>...</div>
    }
    return null;
}
const INavPage = ({ pageNo, currentPage, query, limit, skip }) => {

    if (pageNo === currentPage) {
        return <div className='iNavPageItem--current'> {pageNo} </div>
    } else if(typeof(pageNo) !== 'undefined') {        
        return <a className='iNavPageItem' href={iNav.home(!!query? query:'', limit, skip)}>
            {pageNo}
        </a>
    }
    return null;
}

const INavPagination = ({ paging }) =>  {    
     return <div className='iNavPagination'>
            <INavPageNavigator text={' < '} { ...paging.previous } show={paging.previous}/>
            <INavPage { ...paging.first } currentPage={paging.currentPageNo} limit={paging.limit}/> 
            <INavPageSeparator show={paging.showInitialSeparator} />
            {paging.pages.map((page) => {
                return <INavPage  key={page.pageNo} { ...page } currentPage={paging.currentPageNo} limit={paging.limit}/>
            })}
            <INavPageSeparator show={paging.showTrailingSeparator} />
            <INavPage  { ...paging.last } currentPage={paging.currentPageNo} limit={paging.limit}/>
            <INavPageNavigator text={' > '} { ...paging.next } show={paging.next}/>
            <div className='iNavPageSeparator'> { paging.displayText }</div>
        </div>
}

// Redux Connect
const mapStateToProps = (state) => {
    return {
        paging: state.iNav.paging
    }
}

// Connect the Component to the store
const INavPaginationContainer = connect(
    mapStateToProps
)(INavPagination)

export default INavPaginationContainer