export const proxy = '/editorial/api/v1/proxy/?uri=';
export const iNav = {
    api: '/editorial/api/v1/search/nav',
    aspect: '/editorial/api/v1/search/nav/aspects/',
    home: (query = '', offset = 0, sortOrder = 'Latest') => `/editorial/beta-results/?q=${query}&offset=${offset}&sortOrder=${sortOrder}`,
<<<<<<< HEAD
    refinement: (aspect, refinementAspect, parentExpression, query='')=>`/editorial/api/v1/search/nav/aspects/${aspect}/refinements/${query}&refinementAspect=${refinementAspect}&parentExpression=${parentExpression}`
=======
    refinement: (aspect, refinementAspect, parentExpression, query='')=>`/editorial/api/v1/search/nav/aspects/${aspect}/refinements/?${query}&refinementAspect=${refinementAspect}&parentExpression=${parentExpression}`
>>>>>>> feature/EDITORIAL-246-editorial-desktop-keyword-search
}