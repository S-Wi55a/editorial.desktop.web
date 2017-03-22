const container = (data) => {

    if (!Array.isArray(data)) { return false }

    const moreArticlesPath = "/editorial/api/v1/spec/?uri=";

    return `
        <div class="spec-module">
            <div class="slideshow slideshow--spec-module">
                <div class="slideshow__container swiper-container">

                    <div class="slideshow__slides swiper-wrapper">
                        ${data.map(item => `
                            <div class="slideshow__slide swiper-slide" data-spec-url="${moreArticlesPath}${item.uri}"></div>
                        `).join('')}   
                    </div>

                    <div class="slideshow__pagination-label slideshow__pagination-label--min"></div>
                    <div class="slideshow__pagination swiper-pagination"></div>
                    <div class="slideshow__pagination-label slideshow__pagination-label--max"></div>

                </div>
            </div>
        </div>
     `
}


const item = (data) => {

    return `
        <div class="slideshow__slide swiper-slide"></div>
    `

}


export { container, item };