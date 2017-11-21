import React from 'react'
import { connect } from 'react-redux'
import { Thunks } from 'iNav/Actions/actions'

if (!SERVER) {
    require('iNavPagination/Css/iNavPagination.scss')  
}

const INavPageNavigator = ({ text, url, show, direction, fetchQuery }) => {
    if(show){
        return <a className={`iNavPageItem iNavPageItem--${direction}`} href={url} onClick={(e)=>{e.preventDefault();fetchQuery(url);}}>
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
const INavPage = ({ pageNo, currentPage, url, fetchQuery }) => {

    if(typeof(pageNo) !== 'undefined') {        
        return <a className={`iNavPageItem ${pageNo === currentPage ? 'iNavPageItem--current' : '' }`} href={url} onClick={(e)=>{e.preventDefault();fetchQuery(url);}} data-webm-clickvalue={`${pageNo}`}>
            {pageNo}
        </a>
    }
    return null;
}

const INavPagination = ({ paging, fetchQuery }) =>  {    
     return <div className='iNavPagination' data-webm-section="pagination">
                <div className='iNavPagination__container'>
                    <INavPageNavigator { ...paging.previous } show={paging.previous} direction={'previous'} fetchQuery={fetchQuery} data-webm-clickvalue="previous"/>
                    <INavPage { ...paging.first } currentPage={paging.currentPageNo} fetchQuery={fetchQuery} data-webm-clickvalue="first"/> 
                    <INavPageSeparator {...paging} text='...' initial={true}/>
                    {paging.pages.map((page) => {
                        return <INavPage  key={page.pageNo} { ...page } currentPage={paging.currentPageNo} fetchQuery={fetchQuery}/>
                    })}
                    <INavPageSeparator {...paging} text='...' trailing={true}/>                    
                    <INavPage  { ...paging.last } currentPage={paging.currentPageNo} fetchQuery={fetchQuery} data-webm-clickvalue="last"/>
                    <INavPageNavigator { ...paging.next } show={paging.next}  direction={'next'} fetchQuery={fetchQuery} data-webm-clickvalue="next"/>
                </div>
                <div className='iNavPagination__info'> { paging.displayText }</div>
            </div>
}



// Redux Connect
const mapStateToProps = (state) => {
    return {
        paging: state.store.listings.paging
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        fetchQuery: (query) => {
            dispatch(Thunks.fetchINavAndResults(query));
        }
    }
}

// Connect the Component to the store
const INavPaginationContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(INavPagination)

export default INavPaginationContainer