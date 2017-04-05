const template = (data, startingIndex) => {

    startingIndex = parseInt(startingIndex)

    const items = (str, item, index) => {
        if (index <= 4) {
            return `
                <div class="slideshow__slide swiper-slide">
                    <img class="slideshow__image"
                            src="${item.url}?width=1024&height=683"
                            alt="${item.alternateText || ''}" />
                </div>
            `
        } else {
            return `
                <div class="slideshow__slide swiper-slide">
                    <img class="slideshow__image"
                            data-src="${item.url}width=1024&height=683"
                            alt="${item.alternateText || ''}" />
                </div>
            `
        }
    }

    const container = (data) => `
            <div class="slideshow slideshow--modal swiper-container" data-slideshow-modal data-slideshow-start="${startingIndex}">
        
                <div class="slideshow__slides swiper-wrapper">
                    ${data.map((item, index) =>
                        items`${item}${index}`
                    ).join('')}
                </div>
        
                <div class="slideshow__pagination swiper-pagination"></div>

                <button class="slideshow__nav slideshow__nav--prev" type="button" data-direction="prev">prev</button>
                <button class="slideshow__nav slideshow__nav--next" type="button" data-direction="next">next</button>

            </div>
        `

    return container(data)
}


export { template }