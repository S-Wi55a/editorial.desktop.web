import ScrollMagic from 'ScrollMagic'
import ResizeSensor from 'css-element-queries/src/ResizeSensor'
import * as Utils from 'Features/Details/Assets/StickySidebar/stickySidebar.utils.js'
import { scrollingUp, scrollingDown, scrollingSimple } from 'Features/Details/Assets/StickySidebar/stickySidebar.Actions.js'

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

    let sceneSimple = new ScrollMagic.Scene({
            triggerElement: aside,
            triggerHook: 1 - (w.innerHeight - references.siteNavHeight)/w.innerHeight,
        })  
        .addTo(w.scrollMogicController);
  
    let sceneDown = new ScrollMagic.Scene({
            triggerElement: aside,
            triggerHook: Utils.elementScreenRealEstate('.more-articles').heightInPercentage,
            offset: aside.offsetHeight 
        })
        .addTo(w.scrollMogicController);

    let sceneUp = new ScrollMagic.Scene({
            triggerElement: aside,
            triggerHook: 1 - (w.innerHeight - references.siteNavHeight)/w.innerHeight,
        })       
        .addTo(w.scrollMogicController);

    const boundScrollingSimple = (e)=>{scrollingSimple(pin, aside, references, e)}
    const boundScrollingDown = (e)=>{scrollingDown(pin, aside, references, e)}
    const boundScrollingUp = (e)=>{scrollingUp(pin, aside, references, e)}

    //Check ScreenSize
    function screenSizeCheck() {
        if (aside.offsetHeight < window.innerHeight) {
            sceneDown.off('update', boundScrollingDown) 
            sceneUp.off('update', boundScrollingUp) 
            sceneSimple.on('update', boundScrollingSimple)
        } 
        else {
            sceneSimple.off('update', boundScrollingSimple)
            sceneDown.on('update', boundScrollingDown) 
            sceneUp.on('update', boundScrollingUp) 
        }
    }

    screenSizeCheck();

    //As modules load in the sidebar the hight grows, ResizeSensor has a callback when the size changes
    new ResizeSensor(aside, function() {
        sceneDown.offset(aside.offsetHeight)
        screenSizeCheck();
    });

    // handle event
    window.addEventListener("optimizedResize", screenSizeCheck);

    // For Debugging
    if (process.env.DEBUG) {
        w.sceneDown = sceneDown
        w.sceneUp = sceneUp
        w.sceneSimple = sceneSimple


        sceneDown.addIndicators({ name: "sceneDown" })   
        sceneUp.addIndicators({ name: "sceneUp" })   
        sceneSimple.addIndicators({ name: "sceneSimple" })   


    }    
}