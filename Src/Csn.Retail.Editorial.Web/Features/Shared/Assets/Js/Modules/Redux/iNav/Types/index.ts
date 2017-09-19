
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
    multiSelectMode: string
    facets: IFacet[]
    name: string
    displayName: string
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
    isRefineable: boolean
    refinement: IRefinement
}

export interface IRefinement {
    aspect: string
    parentExpression: string
}

export interface SearchResults { }