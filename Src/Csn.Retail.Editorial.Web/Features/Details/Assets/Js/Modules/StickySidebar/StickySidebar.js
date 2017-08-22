import ScrollMagic from 'ScrollMagic'
import ResizeSensor from 'css-element-queries/src/ResizeSensor'
import * as Utils from 'Features/Details/Assets/Js/Modules/StickySidebar/stickySidebar.utils.js'
import { scrollingUp, scrollingDown } from 'Features/Details/Assets/Js/Modules/StickySidebar/stickySidebar.Actions.js'

if (process.env.DEBUG) { require('debug.addIndicators'); }

export function init(d, w, aside) {

    // Vars
    const pin = {state: false}

    //Wrap aside with a refernce wrapper
    const wrapper = aside.parentNode.insertBefore(d.createElement('div'), aside)
    wrapper.classList.add('scrollmagic-pin-wrapper--aside');
    wrapper.appendChild(aside);

    //Module Vars
    const references = {
        wrapper,
        startingCoordinatesTop : () => { return wrapper.getBoundingClientRect().top - d.querySelector('body').getBoundingClientRect().top },
        siteNavHeight: d.querySelector('.site-nav-wrapper').offsetHeight,
        footerCoordinatesTop : () => { return d.querySelector('#page-footer').getBoundingClientRect().top - d.querySelector('body').getBoundingClientRect().top  }
    }
         
    // Scroll Magic Controller
    w.scrollMogicController = w.scrollMogicController || new ScrollMagic.Controller();
  
    let sceneDown = new ScrollMagic.Scene({
            triggerElement: aside,
            triggerHook: Utils.elementScreenRealEstate('.more-articles').heightInPercentage,
            offset: aside.offsetHeight 
        })
        .on('update', (e)=>{scrollingDown(pin, aside, references, e)}) 
        .addTo(w.scrollMogicController);

    let sceneUp = new ScrollMagic.Scene({
            triggerElement: aside,
            triggerHook: 1 - (w.innerHeight - references.siteNavHeight)/w.innerHeight,
        })
        .on('update', (e)=>{scrollingUp(pin, aside, references, e)}) 
        .addTo(w.scrollMogicController);

    //As modules load in the sidebar the hight grows, ResizeSensor has a callback when the size changes
    new ResizeSensor(aside, function() {
        sceneDown.offset(aside.offsetHeight)
    });

    // handle event
    window.addEventListener("optimizedResize", function() {
    });

    // For Debugging
    if (process.env.DEBUG) {
        w.sceneDown = sceneDown
        w.sceneUp = sceneUp

        sceneDown.addIndicators({ name: "sceneDown" })   
        sceneUp.addIndicators({ name: "sceneUp" })   

    }    
}