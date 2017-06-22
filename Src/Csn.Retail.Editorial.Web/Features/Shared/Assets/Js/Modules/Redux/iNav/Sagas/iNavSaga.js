import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes'
import { delay } from 'redux-saga'
import { call, put, takeEvery, takeLatest, all } from 'redux-saga/effects'
import fetch from 'isomorphic-fetch'


const Api = {
    fetchiNav : (query) => {
        return fetch(`/editorial/api/v1/search?q=${query}`)
            .then(response => response.json())
    }
}


// Our worker Saga: will fetch our data
export function* fetchData(action) {
    try {
        const data = yield call(Api.fetchiNav, action.payload.query)
        yield put({type: ActionTypes.FETCH_QUERY_SUCCESS, data})
    } catch (error) {
        yield put({type: ActionTypes.FETCH_QUERY_FAILURE, error})
    }
}

// Our watcher Saga: spawn a new incrementAsync task on each INCREMENT_ASYNC
export function* watchFetchData() {
    yield takeLatest(ActionTypes.FETCH_QUERY_REQUEST, fetchData)
}