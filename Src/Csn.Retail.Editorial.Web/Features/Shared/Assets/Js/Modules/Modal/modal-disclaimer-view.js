//Basic Disclaimer Modal View

// Optional className
// This view forces a single <p>. Should be used for simple one paragrpah disclaimers
export const disclaimerTemplate = function(disclaimerText, className) {
    return `
        <div class="${className || ''} csn-modal">
            <div class="csn-modal__content">
                <p>${disclaimerText || ''}</p>
            </div>
        </div>
    `;
}