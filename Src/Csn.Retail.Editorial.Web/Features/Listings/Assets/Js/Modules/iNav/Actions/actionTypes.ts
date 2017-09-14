﻿export enum API {
    TOGGLE_IS_SELECTED = 'INAV/TOGGLE_IS_SELECTED', //UI & data
    REMOVE_BREAD_CRUMB = 'INAV/REMOVE_BREAD_CRUMB', //UI & data
    RESET = 'INAV/RESET',
    UPDATE_QUERY_STRING = 'INAV/UPDATE_QUERY_STRING',

    // Query
    FETCH_QUERY_REQUEST = 'INAV/FETCH_QUERY_REQUEST',
    FETCH_QUERY_SUCCESS = 'INAV/FETCH_QUERY_SUCCESS',
    FETCH_QUERY_FAILURE = 'INAV/FETCH_QUERY_FAILURE',

    // UI //TODO: move out or have Action Types specfifically for UI
    TOGGLE_IS_ACTIVE = 'TOGGLE_IS_ACTIVE',
    SHOW_LOADER = 'SHOW_LOADER',
    HIDE_LOADER = 'HIDE_LOADER'
}

export enum UI {  
    // UI //TODO: move out or have Action Types specfifically for UI
    TOGGLE_IS_ACTIVE = 'UI/TOGGLE_IS_ACTIVE',
  }