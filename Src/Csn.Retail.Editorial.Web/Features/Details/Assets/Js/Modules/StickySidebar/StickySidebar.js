import ScrollMagic from 'ScrollMagic'
import ResizeSensor from 'css-element-queries/src/ResizeSensor'
import * as Utils from 'Features/Details/Assets/Js/Modules/StickySidebar/stickySidebar.utils.js'
import { scrollingUp, scrollingDown, scrollingSimple } from 'Features/Details/Assets/Js/Modules/StickySidebar/stickySidebar.Actions.js'

if (process.env.DEBUG) { require('debug.addIndicators'); }

export function init(d, w, aside) {

    //Wrap aside with a refernce wrapper
    const wrapper = aside.parentNode.insertBefore(d.createElement('div'), aside)
    wrapper.classList.add('scrollmagic-pin-wrapper--aside');
    wrapper.appendChild(aside);

    //Module Vars
    const references = {
        wrapper,
        startingCoordinatesTop : wrapper.getBoundingClientRect().top,
        siteNavHeight: d.querySelector('.site-nav-wrapper').offsetHeight,
        footerCoordinatesTop : () => { return d.querySelector('#page-footer').getBoundingClientRect().top - wrapper.getBoundingClientRect().top },
        triggerHookUp: ()=> 1 - (w.innerHeight - d.querySelector('.site-nav-wrapper').offsetHeight) / w.innerHeight,
        triggerHookDown : ()=> Utils.elementScreenRealEstate('.more-articles').heightInPercentage
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
            triggerHook: references.triggerHookDown(),
            offset: aside.offsetHeight 
        })
        .addTo(w.scrollMogicController);

    let sceneUp = new ScrollMagic.Scene({
            triggerElement: aside,
            triggerHook: references.triggerHookUp(),
        })       
        .addTo(w.scrollMogicController);

    const boundScrollingSimple = (e)=>{scrollingSimple(aside, references, e)}
    const boundScrollingDown = (e)=>{scrollingDown(aside, references, e)}
    const boundScrollingUp = (e)=>{scrollingUp(aside, references, e)}

    //Check ScreenSize
    function screenSizeCheck() {
        if(aside.offsetHeight > (document.querySelector('article .article') ? document.querySelector('article .article').offsetHeight : 0)){
            sceneDown.off('update', boundScrollingDown) 
            sceneUp.off('update', boundScrollingUp) 
            sceneSimple.off('update', boundScrollingSimple)
        }
        else if (aside.offsetHeight < window.innerHeight) {
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
        sceneUp.triggerHook(1 - (w.innerHeight - references.siteNavHeight) / w.innerHeight) // to trigger update
        sceneDown.trigger('update')
        screenSizeCheck();
    });
    if(document.querySelector('#disqus_thread')){
        new ResizeSensor(document.querySelector('#disqus_thread'), function() {
            sceneDown.trigger('update')
        })
    }

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