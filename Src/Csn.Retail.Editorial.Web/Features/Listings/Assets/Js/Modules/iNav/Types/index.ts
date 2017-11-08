
//NOTE: This State should sit else where
export interface State {
    store: IStore
}

export interface IStore {
    listings: INavResults
    history: { listings: INavResults }
}

export interface INavResults {
    navResults: IINavResponse
}

export interface IINavResponse{
    iNav: IINav
    count?: number
    pendingQuery?: string
    currentRefinement?: string
    searchResults: ISearchResults[]
}

export interface IINav {
    nodes: INode[]
    breadCrumbs: Array<any>
    metadata: INavMetadata
}

export interface INavMetadata {

}

export interface INode {
    count?: number
    multiSelectMode: string
    facets: IFacet[]
    name: string
    displayName: string
    refinements?: IRefinements
}

export interface IRefinements {
    facets: IFacet[]
    displayName: string
}

export interface INodeMetadata {

}

export interface IFacet {
    isSelected: boolean
    value: string
    displayValue: string
    action: string
    count: number
    expression: string
    isRefineable: boolean
    refinement?: IRefinement
    refinements?: IRefinements
    
}

export interface IRefinement {
    aspect: string
    parentExpression: string
}

export interface ISearchResults {
    imageUrl: string
    headline: string
    subHeading: string
    dateAvailable: string
    articleDetailsUrl: string
    label: string
    type: string
    disqusArticleId: string | number
}