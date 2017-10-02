export const proxy = '/editorial/api/v1/proxy/?uri=';
export const iNav = {
    base: '/editorial/api/v1/search/nav?q=',
    aspect: '/editorial/api/v1/search/nav/aspects/',
    refinement: (aspect, refinementAspect, parentExpression, query='')=>`/editorial/api/v1/search/nav/aspects/${aspect}/refinements/?refinementAspect=${refinementAspect}&parentExpression=${parentExpression}&q=${query}`
}