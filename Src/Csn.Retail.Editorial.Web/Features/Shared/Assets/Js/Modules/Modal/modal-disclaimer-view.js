//Basic Disclaimer Modal View

// Optional className
export const disclaimerTemplate = function(text, className) {
    return `
        <div class="${className || ''} csn-modal">
            <div class="csn-modal__content">
                ${text || ''}
            </div>
        </div>
    `;
}
