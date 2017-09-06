import * as ActionTypes from 'Redux/iNav/Actions/actionTypes'
import * as iNav from 'Redux/iNav/Actions/actionTypes'
import { delay } from 'redux-saga'
import { call, put, takeEvery, takeLatest, all } from 'redux-saga/effects'
import fetch from 'isomorphic-fetch'
import * as endPoint from 'Endpoints/endpoints'


const Api = {
    fetchiNav : (query = '') => {
        console.log(`${endPoint.iNav}${query}`)
        return fetch(`${endPoint.iNav}${query}`)
            .then(response => response.json())
    }
}

// Our worker Saga: will fetch our data
export function* fetchData(action) {
    try {
        //show loader
        yield put({ type: iNav.SHOW_LOADER })
        const data = yield call(Api.fetchiNav, action.payload.query)
        yield put({ type: ActionTypes.FETCH_QUERY_SUCCESS, data })
    } catch (error) {
        yield put({ type: ActionTypes.FETCH_QUERY_FAILURE, error })
    } finally {
        //Hide loader
        yield put({ type: iNav.HIDE_LOADER })
  
    }
}

// Our watcher Saga
export function* watchINavSagaActions() {
    yield takeLatest(
        [
            ActionTypes.FETCH_QUERY_REQUEST,
            ActionTypes.RESET
        ], 
        fetchData)
}


