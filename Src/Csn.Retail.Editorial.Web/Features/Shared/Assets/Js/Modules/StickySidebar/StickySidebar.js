﻿import ScrollMagic from 'ScrollMagic'
import ResizeSensor from 'css-element-queries/src/ResizeSensor'
import { scrollingUp, scrollingDown, scrollingSimple } from 'StickySidebar/stickySidebar.Actions.js'

if (process.env.DEBUG) { require('debug.addIndicators'); }

export function init(d, w, aside, selector, baseReference, topReference) {

    if (typeof d === 'undefined' ||  typeof w === 'undefined' ||  typeof aside === 'undefined' ||  typeof baseReference === 'undefined') {
        console.error('Please check if you are passing the correct arguments to init')
        return
    }

    // Cache Footer
    const pageFooter = d.querySelector('#page-footer')
    const siteNavHeight = topReference ? topReference : d.querySelector('.site-nav-wrapper') ? d.querySelector('.site-nav-wrapper').offsetHeight : 40
    const wrapper = d.querySelector('.wrapper--aside')
    
    //Module Vars
    const references = {
        wrapper,
        startingCoordinatesTop : wrapper.getBoundingClientRect().top,
        siteNavHeight: siteNavHeight,
        footerCoordinatesTop : () => { return d.querySelector('#page-footer').getBoundingClientRect().top - wrapper.getBoundingClientRect().top },
        triggerHookUp: ()=> 1 - (w.innerHeight - siteNavHeight) / w.innerHeight,
        triggerHookDown : (w.innerHeight - baseReference) / w.innerHeight,
        pageFooter,
        baseReference
    }
         
    // Scroll Magic Controller
    w.scrollMogicController = w.scrollMogicController || new ScrollMagic.Controller();

    let sceneSimple = new ScrollMagic.Scene({
            triggerElement: aside,
            triggerHook: 1 - (w.innerHeight - references.siteNavHeight) / w.innerHeight,
        })  
        .addTo(w.scrollMogicController);

    let sceneDown = new ScrollMagic.Scene({
            triggerElement: aside,
            triggerHook: references.triggerHookDown,
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
        if(aside.offsetHeight > (document.querySelector(selector) ? document.querySelector(selector).offsetHeight : 0)){           
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
        screenSizeCheck();
    });
    if(document.querySelector('#disqus_thread')){
        new ResizeSensor(document.querySelector('#disqus_thread'), function() {
            sceneDown.triggerHook(references.triggerHookDown) //TODO: check if this works correctly
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