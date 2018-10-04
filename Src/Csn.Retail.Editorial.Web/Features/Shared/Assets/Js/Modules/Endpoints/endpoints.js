export const proxyEndpoint = () => {
    if (typeof csn_editorial !== 'undefined' || typeof csn_editorial.endpoints !== 'undefined') {
        return `${csn_editorial.endpoints.proxyApiUrl}?uri=`;
    }

    return '';
}

export const iNavEndpoints = () => {
    var endpoints = {};

    if (typeof csn_editorial !== 'undefined' && typeof csn_editorial.endpoints !== 'undefined') {
        endpoints.nav = csn_editorial.endpoints.navApiUrl;
        endpoints.home = (query = '', offset = 0, sortOrder = 'Latest') => `${csn_editorial.endpoints.searchResultsUrl}?q=${query}&offset=${offset}&sortOrder=${sortOrder}`;
        endpoints.refinement = (refinementAspect, parentExpression, query = '') => `${csn_editorial.endpoints.navRefinementApiUrl}${query.length ? query : '?'}&refinementAspect=${refinementAspect}&parentExpression=${parentExpression}`;
    }

    return endpoints;
}