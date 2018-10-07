var proxyApiUrl = '';
var navApiUrl = '';
var navRefinementApiUrl = '';
var searchResultsUrl = '';

if (typeof csn_editorial !== 'undefined' && typeof csn_editorial.endpoints !== 'undefined') {
    proxyApiUrl = csn_editorial.endpoints.proxyApiUrl;
    navApiUrl = csn_editorial.endpoints.navApiUrl;
    navRefinementApiUrl = csn_editorial.endpoints.navRefinementApiUrl;
    searchResultsUrl = csn_editorial.endpoints.searchResultsUrl;
}

export const proxyEndpoint = `${proxyApiUrl}?uri=`;

export const iNavEndpoints = {
    nav: navApiUrl,
    home: (query = '', offset = 0, sortOrder = 'Latest') => `${searchResultsUrl}?q=${query}&offset=${offset}&sortOrder=${sortOrder}`,
    refinement: (refinementAspect, parentExpression, query = '') => `${navRefinementApiUrl}${query.length ? query : '?'}&refinementAspect=${refinementAspect}&parentExpression=${parentExpression}`
}