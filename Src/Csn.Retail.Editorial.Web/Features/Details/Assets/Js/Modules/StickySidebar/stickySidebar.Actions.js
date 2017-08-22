import * as Utils from 'Features/Details/Assets/Js/Modules/StickySidebar/stickySidebar.utils.js'

const lock = { up: false }

export function scrollingUp(pin, el, ref, e) {

    const state = {
        STATE: e.target.state(),
        FORWARD: e.target.controller().info('scrollDirection') === 'FORWARD',
        REVERSE: e.target.controller().info('scrollDirection') === 'REVERSE',
        REACHED_TOP: e.scrollPos <= ref.startingCoordinatesTop(),
    }
    if (state.FORWARD) {
        lock.up = false; // the lock is for debouncing
    }

    if (state.REVERSE) {

        if (process.env.DEBUG) { console.log("Scence Up:: pinState: " + pin.state + " State: " + state.STATE + " Reached Top: " + state.REACHED_TOP + " Lock Up: " + lock.up)}

        //Reached Top
        if (state.REACHED_TOP) {
            if (process.env.DEBUG) { console.log('Up 1') }

            const css = {
                top: 'auto',
                position: 'static',
            }
            pin.state = false
            lock.up = false
            Utils.setStylesForElement(el, css)
        }
        // The top trigger has passed the trigger hook
        else if (pin.state && state.STATE === 'DURING' && !lock.up) {
            if (process.env.DEBUG) { console.log('Up 2') }

            const css = {  
                top: Utils.distance(ref.wrapper.getBoundingClientRect().top, el.getBoundingClientRect().top, ref.startingCoordinatesTop()) + 'px',
                position: 'absolute',
            }
            pin.state = false
            Utils.setStylesForElement(el, css)

        }
        // The top trigger hasnot  passed the trigger hook
        else if (state.STATE === 'BEFORE' && !state.REACHED_TOP && !lock.up) {
            if (process.env.DEBUG) { console.log('Up 3') }

            const css = {
                top: ref.siteNavHeight + 'px',
                position: 'fixed'
            }
            pin.state = true
            lock.up = true
            Utils.setStylesForElement(el, css)
        
        }
    } 

}

export function scrollingDown(pin, el, ref, e) {

    const state = {
        STATE: e.target.state(),
        FORWARD: e.target.controller().info('scrollDirection') === 'FORWARD', 
        REACHED_BOTTOM: (e.scrollPos + el.getBoundingClientRect().bottom) >= ref.footerCoordinatesTop()
    }

    if (state.FORWARD) {

        // Update Trigger hook when more article height changes
        e.target.triggerHook(Utils.elementScreenRealEstate('.more-articles').heightInPercentage)

        if (process.env.DEBUG) { console.log("Scence Down:: pinState: " + pin.state + " State: " + state.STATE + " Reached Bottom: " + state.REACHED_BOTTOM) }
        
        //Reached footer
        if (state.REACHED_BOTTOM) {
            if (process.env.DEBUG) { console.log('Down 1') }
               
            const css = {  
                top: ref.footerCoordinatesTop() - el.offsetHeight  + 'px',
                position: 'absolute'
            }
            pin.state = false
            Utils.setStylesForElement(el, css)

        }
        // The bottom trigger has passed the trigger hook
        else if (state.STATE === 'DURING') {
            if (process.env.DEBUG) { console.log('Down 2') }

            const css = {
                top: (window.innerHeight - Utils.elementScreenRealEstate('.more-articles').height) - el.offsetHeight + 'px',
                position: 'fixed'
            }
            pin.state = true
            Utils.setStylesForElement(el, css)

        }
        // The bottom trigger has not passed the trigger hook
        else if (state.STATE === 'BEFORE') {
            if (process.env.DEBUG) { console.log('Down 3') }

            const css = {  
                top: Utils.distance(ref.wrapper.getBoundingClientRect().top, el.getBoundingClientRect().top, ref.startingCoordinatesTop()) + 'px',
                position: 'absolute'
            }
            pin.state = false
            Utils.setStylesForElement(el, css)

        }
    }

}

   
            
  





