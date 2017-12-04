export const proxy = '/editorial/api/v1/proxy/?uri=';
export const iNav = {
    nav: '/editorial/api/v1/search/nav',
    home: (query = '', offset = 0, sortOrder = 'Latest') => `/editorial/results/?q=${query}&offset=${offset}&sortOrder=${sortOrder}`,
    refinement: (refinementAspect, parentExpression, query='')=>`/editorial/api/v1/search/nav/refinements/${ query.length ? query : '?' }&refinementAspect=${refinementAspect}&parentExpression=${parentExpression}`
}