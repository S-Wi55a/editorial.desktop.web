const container = (data) => {

    if (!Array.isArray(data)) { return false }

    const moreArticlesPath = "/editorial/api/v1/spec/?uri=";

    return `
        <div class="spec-module">
            <div class="slideshow slideshow--spec-module">
                <div class="slideshow__container  swiper-container">

                    <div class="slideshow__slides swiper-wrapper">
                        ${data.map(item => `
                            <div class="slideshow__slide swiper-slide"></div>
                        `).join('')}   
                    </div>

                    <div class="slideshow__pagination-label slideshow__pagination-label--min">
                    <div class="slideshow__pagination swiper-pagination"></div>
                    <div class="slideshow__pagination-label slideshow__pagination-label--max">

                </div>
            </div>
        </div>
     `
}


const item = (data) => {

    return `
                    <div class="slideshow__slide swiper-slide">
                        <a href="@Url.Action("Images", "Details", new { imageIndex = index })" data-modal-trigger data-modal-type="slideshow">

                            <img class="slideshow__image"
                                 sizes="100vw"
                                 src="@image.Url?width=1024&height=683"
                                 srcset="@image.Url?width=1024&height=683 2048w, @image.Url?width=768&height=512 1536w, @image.Url?width=512&height=341 1024w, @image.Url?width=384&height=256 768w"
                                 alt="@image.AlternateText" />
                        </a>

    `

}


export { container, item };