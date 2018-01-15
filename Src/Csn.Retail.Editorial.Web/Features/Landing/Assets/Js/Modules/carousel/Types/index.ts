export interface IState {
    carousels: ICarouselViewModel[]
}
export interface ICarouselViewModel {
    title: string
    carouselItems: ICarouselItems[]
    viewAllLink: string
    hasMrec: boolean
    nextQuery: string
    polarAds: IPolarAds
}
export interface IPolarAds {
    display: boolean
    placementId: number
}
export interface ICarouselItems {
    imageUrl: string
    headline: string
    subHeading: string
    dateAvailable: string
    articleDetailsUrl: string
    label: string
    type: string
    disqusArticleId: string | number
    itemUrl?: string
}

export interface ISimpleSlider {
    carouselItems: ICarouselItems[]
    hasMrec: boolean
    nextQuery: string
    index: number
    fetch: (q:string, i:number) => any
    polarAds: IPolarAds
    title: string
}