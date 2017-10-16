import React from 'react'
import { connect } from 'react-redux'
import { Thunks } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'

if (!SERVER) {
    require('iNavPagination/Css/iNavPagination.scss')  
}

const INavPageNavigator = ({ text, limit, query, show, skip, direction, sortOrder }) => {    
    if(!!show){
        return <a className={`iNavPageItem iNavPageItem--${direction}`} href={iNav.home(!!query? query:'', limit, skip, sortOrder.value)}>
            {text}
        </a>
    }
    return null;
}
const INavPageSeparator = ({ initial, trailing, totalPageCount, currentPageNo, text }) => {    
    if (initial && totalPageCount > 4 && currentPageNo > 2) {
        return <div className='iNavPageItem iNavPageItem--separator'>{text}</div>
    }
    else if (trailing && totalPageCount > 4 && currentPageNo < totalPageCount - 2) {
        return <div className='iNavPageItem iNavPageItem--separator'>{text}</div>
    }
    return null;
}
const INavPage = ({ pageNo, currentPage, query, limit, skip, sortOrder }) => {

    if (pageNo === currentPage) {
        return <div className='iNavPageItem iNavPageItem--current'> {pageNo} </div>
    } else if(typeof(pageNo) !== 'undefined') {        
        return <a className='iNavPageItem' href={iNav.home(!!query? query:'', limit, skip, sortOrder.value)}>
            {pageNo}
        </a>
    }
    return null;
}

const INavPagination = ({ paging, sortOrder }) =>  {    
     return <div className='iNavPagination'>
                <div className='iNavPagination__container'>
                    <INavPageNavigator { ...paging.previous } show={paging.previous} direction={'previous'} sortOrder={sortOrder}/>
                    <INavPage { ...paging.first } currentPage={paging.currentPageNo} limit={paging.limit} sortOrder={sortOrder}/> 
                    <INavPageSeparator {...paging} text='...' initial={true}/>
                    {paging.pages.map((page) => {
                        return <INavPage  key={page.pageNo} { ...page } currentPage={paging.currentPageNo} limit={paging.limit} sortOrder={sortOrder}/>
                    })}
                    <INavPageSeparator {...paging} text='...' trailing={true}/>                    
                    <INavPage  { ...paging.last } currentPage={paging.currentPageNo} limit={paging.limit} sortOrder={sortOrder}/>
                    <INavPageNavigator { ...paging.next } show={paging.next}  direction={'next'} sortOrder={sortOrder}/>
             {sortOrder.value}
                </div>
                <div className='iNavPagination__info'> { paging.displayText }</div>
            </div>
}

// Redux Connect
const mapStateToProps = (state) => {
    return {
        paging: state.iNav.paging,
        sortOrder: state.iNav.sorting.sortListItems.filter((sortOrder) => {
            return sortOrder.selected === true;
        })[0]
    }
}

// Connect the Component to the store
const INavPaginationContainer = connect(
    mapStateToProps
)(INavPagination)

export default INavPaginationContainer