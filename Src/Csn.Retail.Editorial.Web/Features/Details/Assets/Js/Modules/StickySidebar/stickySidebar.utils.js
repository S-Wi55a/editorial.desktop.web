// Helper Functions
export function elementScreenRealEstate(selector) {
    //check if more articles is loaded if not use default values
    const el = document.querySelector(selector)
    const height =  el ? window.innerHeight - el.getBoundingClientRect().top : 0

    return {
        height,
        heightInPercentage: (window.innerHeight - height) / window.innerHeight
    }
}

export function setStylesForElement(el, css) {
    let newStyle = ''
    for (var prop in css) {
        if (css.hasOwnProperty(prop)) {
            newStyle += ` ${prop}:${css[prop]};`
        } 
    }
    if( typeof( el.style.cssText ) != 'undefined' ) {
        el.style.cssText += newStyle; // Use += to preserve existing styles
      } else {
        el.setAttribute('style',newStyle);
      }
    
}

export function distanceFromStartingPoint(scene, elPosition, ref, offset = 0) {
    const scrollPosition = scene.controller().info().scrollPos
    const referenceStartingPoint = Math.abs(document.querySelector('body').getBoundingClientRect().top - ref.getBoundingClientRect().top)
    const elementHeight = offset
  
    return scrollPosition + elPosition - referenceStartingPoint - elementHeight   
}

