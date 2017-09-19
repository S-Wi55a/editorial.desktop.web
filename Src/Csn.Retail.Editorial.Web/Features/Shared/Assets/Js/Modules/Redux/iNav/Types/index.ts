
//NOTE: This State should sit else where
export interface State {
    iNav: IINavResponse
}

export interface IINavResponse{
    iNav: IINav
}

export interface IINav {
    nodes: INode[]
    breadCrumbs: Array<any>
    metadata: INavMetadata
}

export interface INavMetadata {

}

export interface INode {
    isSelected: boolean
    placeholderExpression: string
    multiSelectMode: string
    removeAction: string
    facets: IFacet[]
    metadata: INodeMetadata
    name: string
    displayName: string
    type: string
}

export interface INodeMetadata {

}

export interface IFacet {
    isSelected: boolean
    value: string
    displayValue: string
    action: string
    count: number,
    expression: string,
    metadata: IFacetMetadata
}

export interface IFacetMetadata {
    isRefineable: boolean[]
}

export interface SearchResults { }