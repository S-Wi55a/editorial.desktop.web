export default (data) => {
    const template = data.Items.map((item) => {
        return (`
            <div class="more-article more-articles__slide swiper-slide">
                    <a class="more-article__link-container" href="${item.Url}">
                        <div class="more-article__image">
                            <img src="${item.Image.Url}?width=140&height=93" alt="${item.Image.AlternateText}" />
                        </div>
                        <div class="more-article__content">
                            <div class="more-article__title">
                                <h2 >${item.Headline}</h2>
                            </div>
                            <p class="more-article__link" href="${item.Url}">${data.ReadMore}</p>
                                        ${item.Sponsored ?
                                `<div class="more-article__banner more-article__banner--${item.Sponsored}">${item.Sponsored}</div>`
                            : ''}
                        </div>
                    </a>
                </div>
            `)
    })

    return template.reduce((prev, current) => {
        return prev + current
    })
}