// Helper Functions
export function elementScreenRealEstate(selector) {
    //check if more articles is loaded if not use default values
    const el = document.querySelector(selector)
    const height =  el ? Math.abs((el.getBoundingClientRect().top - window.innerHeight)) : 0 ;

    return {
        height,
        heightInPercentage: (window.innerHeight - height)/window.innerHeight
    }
}

export function distance(startingPos, currentPos, offset) {
    return (currentPos + offset) - startingPos
}

export function setStylesForElement(el, css) {
    for (var prop in css) {
        if (css.hasOwnProperty(prop)) {
            el.style[prop] = css[prop]
        } 
    }
    
}
