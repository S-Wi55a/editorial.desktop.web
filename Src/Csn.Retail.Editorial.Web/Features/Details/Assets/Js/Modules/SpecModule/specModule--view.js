const disclaimer = (data) => {

    return `
        <div class="spec-module-disclaimer">
            <div class="spec-module-disclaimer__content">
                <p>${data || ''}</p>
            </div>
        </div>
    `
}

export { disclaimer };