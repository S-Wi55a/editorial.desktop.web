export const proxy = '/editorial/api/v1/proxy/?uri=';
export const iNav = {
    base: '/editorial/api/v1/search/nav',
    aspect: '/editorial/api/v1/search/nav/aspects/',
    home:(query='', limit=20, skip=0, sortOrder='Latest') => `/editorial/beta-results/?q=${query}&limit=${limit}&skip=${skip}&sortOrder=${sortOrder}`,
    refinement: (aspect, refinementAspect, parentExpression, query='')=>`/editorial/api/v1/search/nav/aspects/${aspect}/refinements/?refinementAspect=${refinementAspect}&parentExpression=${parentExpression}&q=${query}`
}