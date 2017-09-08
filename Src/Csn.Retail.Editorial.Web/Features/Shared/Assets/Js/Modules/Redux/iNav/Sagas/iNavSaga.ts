import { Actions , ActionTypes } from 'Redux/iNav/Actions/actions'
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
export function* fetchData(action: Actions) {
    try {
        //show loader
        yield put({ type: ActionTypes.SHOW_LOADER }) //UI Specific actions
        const data = yield call(Api.fetchiNav, action.payload.query)
        yield put({ type: ActionTypes.FETCH_QUERY_SUCCESS, data })
    } catch (error) {
        yield put({ type: ActionTypes.FETCH_QUERY_FAILURE, error })
    } finally {
        //Hide loader
        yield put({ type: ActionTypes.HIDE_LOADER }) //UI specific actions
  
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


