﻿export interface IState {
    carousels: ICarouselViewModel[]
}
export interface ICarouselViewModel {
    title: string
    carouselItems: ICarouselItems[]
    viewAllLink: string
    hasMrec: boolean
    nextQuery: string
    hasPolar: boolean
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
}
export interface ICarouselDriverItems {
    imageUrl: string
    itemUrl: string
}