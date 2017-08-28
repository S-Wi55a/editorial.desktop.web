import objectPath from 'object-path'

export const getUI = (path) => {
    if (typeof window === 'object' && window.__PRELOADED_STATE__ !== 'undefined') {
        return objectPath.get(window.__PRELOADED_STATE__, path)
    } else {
        return null
    }
}
