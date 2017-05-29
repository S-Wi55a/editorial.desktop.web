﻿const PREFIX = 'INAV/' //Prefix for iNav actions

// Action Types
export const TOGGLE_SELECTED = PREFIX + 'TOGGLE_SELECTED' //UI & data
export const RESET = PREFIX + 'RESET'
export const UPDATE_QUERY_STRING = PREFIX + 'UPDATE_QUERY_STRING'

// Query
export const FETCH_QUERY_REQUEST = PREFIX + 'FETCH_QUERY_REQUEST'
export const FETCH_QUERY_SUCCESS = PREFIX + 'FETCH_QUERY_SUCCESS'
export const FETCH_QUERY_FAILURE = PREFIX + 'FETCH_QUERY_FAILURE'

export const MODEL_SELECTED = PREFIX + 'MODEL_SELECTED' //UI - can be internal

//UI
export const TOGGLE_IS_ACTIVE = PREFIX + 'TOGGLE_IS_ACTIVE'