export const proxy = '/editorial/api/v1/proxy/?uri=';
export const iNav = {
    base: '/editorial/api/v1/search/nav?q=',
    aspect: '/editorial/api/v1/search/nav/aspects/',
    home: (query = '', offset = 0, sortOrder = 'Latest') => `/editorial/beta-results/?q=${query}&offset=${offset}&sortOrder=${sortOrder}`,
    refinement: (aspect, refinementAspect, parentExpression, query='')=>`/editorial/api/v1/search/nav/aspects/${aspect}/refinements/?refinementAspect=${refinementAspect}&parentExpression=${parentExpression}&q=${query}`
}