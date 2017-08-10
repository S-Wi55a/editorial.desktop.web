import ScrollMagic from 'ScrollMagic';
if (process.env.DEBUG) { require('debug.addIndicators'); }


export default function(d, w, aside) {

    // Load Rezise sensor
    const ResizeSensor = require('css-element-queries/src/ResizeSensor');

    //Wrap aside with a refernce wrapper
    const wrapper = aside.parentNode.insertBefore(d.createElement('div'), aside)
    wrapper.classList.add('scrollmagic-pin-wrapper--aside');
    wrapper.appendChild(aside);

    //Module Vars
    let pinState = false
    let triggerHookValueCache = null  
    const references = {
        wrapper,
        startingCoordinatesTop : () => { return wrapper.getBoundingClientRect().top - d.querySelector('body').getBoundingClientRect().top },
        calcTriggerHookFromTop: (x) => 1 - (w.innerHeight - x)/w.innerHeight,
        siteNavHeight: d.querySelector('.site-nav-wrapper').offsetHeight,
        footerCoordinatesTop : () => { return d.querySelector('#page-footer').getBoundingClientRect().top - d.querySelector('body').getBoundingClientRect().top  }
    }
         
    // Scroll Magic Controller
    w.scrollMogicController = w.scrollMogicController || new ScrollMagic.Controller();

    //Create Scene
    let stickySidebarScene = new ScrollMagic.Scene({
            triggerElement: wrapper,
            triggerHook: moreArticlesIsVisible().heightInPercentage,
            offset: d.querySelector('body').offsetHeight // We do this to prevent the scene starting early
        })
        .on('update', (e)=>onUpdate(aside, references, e)) 
        .addTo(w.scrollMogicController);

    // For Debugging
    if (process.env.DEBUG) {
        w.stickySidebarScene = stickySidebarScene
        stickySidebarScene.addIndicators({ name: "Sticky Nav" })   
    }

    //As modules load in the sidebar the hight grows, ResizeSensor has a callback when the size changes
    new ResizeSensor(aside, function() {
        stickySidebarScene.offset(aside.offsetHeight)
    });

    // Function called when Scroll Magic updates the scene
    function onUpdate(el, r, e) {

        const SCENE = e.target
        const STATE = SCENE.state()
        const FORWARD = SCENE.controller().info('scrollDirection') === 'FORWARD'
        const REVERSE = !FORWARD
        const EL_COORDINATES = el.getBoundingClientRect()
        const REACHED_BOTTOM = (e.scrollPos + EL_COORDINATES.bottom) >= r.footerCoordinatesTop()
        const moreArticlesVisible = moreArticlesIsVisible()
        
        //We don't know when the more-article container is shown or hidden but we can check the height on scroll
        if (FORWARD && !REACHED_BOTTOM) {

            //If the trigger hook changed, on next scroll set top pos
            if (SCENE.triggerHook() !== triggerHookValueCache) {
                if (pinState) {
                    const css = {
                        top: (w.innerHeight - moreArticlesVisible.height) - el.offsetHeight + 'px',
                        transition: 'top 0.333s'
                    }
                    setStylesForElement(el)(css)  
                }
                triggerHookValueCache = moreArticlesVisible.heightInPercentage
            }
            //Change Trigger Hook position, on happens in forward direction
            SCENE.triggerHook(moreArticlesVisible.heightInPercentage)
        }
         
        //this ensures that when the w is resized the sidebar moves w/ it
        if(el.style.position === 'static') {setStylesForElement(el)({left:r.wrapper.getBoundingClientRect().left + 'px'})} 
            
  
        //DEBUG
        if (process.env.DEBUG) { console.log(pinState, STATE, FORWARD) }

         //Not sticky, scene is active , scrolling down, not the bottom             
        if (!pinState && STATE === 'DURING' && FORWARD && !REACHED_BOTTOM ) {
            if (process.env.DEBUG) { console.log('1') }
               
            const css = {
                top: (w.innerHeight - moreArticlesVisible.height) - el.offsetHeight + 'px',
                position: 'fixed',
                transition: ''
            }
            setState(true, css)

        } 
        //Sticky, scene is active , scrolling up, position fixed 
        else if (pinState && STATE === 'DURING' && REVERSE && el.style.position === 'fixed') {
            if (process.env.DEBUG) { console.log('2') }
                
            const css = {  
                top: distance(r.wrapper.getBoundingClientRect().top, EL_COORDINATES.top, r.startingCoordinatesTop()) + 'px',
                position: 'absolute',
                transition: ''

            }
            setState(false, css)

            SCENE.offset(EL_COORDINATES.top - r.wrapper.getBoundingClientRect().top)
            SCENE.triggerHook(r.calcTriggerHookFromTop(r.siteNavHeight))
        }
        //Not sticky, scene is not active, scrolling up, position absolute
        else if (!pinState && STATE === 'BEFORE' && REVERSE && el.style.position === 'absolute') {
            if (process.env.DEBUG) { console.log('3') }

            const css = {
                top: r.siteNavHeight + 'px',
                position: 'fixed',
                transition: ''
            }
            setState(true, css)
        }
        //Sticky, scene is not active , scrolling down, position fixed
        else if (pinState && STATE === 'BEFORE' && FORWARD && el.style.position === 'fixed') {
            if (process.env.DEBUG) { console.log('4') }

            const css = {  
                top: distance(r.wrapper.getBoundingClientRect().top, EL_COORDINATES.top, r.startingCoordinatesTop()) + 'px',
                position: 'absolute',
                transition: ''
            }
            setState(false, css)
            SCENE.offset(EL_COORDINATES.top - r.wrapper.getBoundingClientRect().top + el.offsetHeight)
            SCENE.triggerHook(moreArticlesVisible.heightInPercentage)
        }
        //If starting Pos is reached
        else if (pinState && e.scrollPos < r.startingCoordinatesTop() - r.siteNavHeight) {
            if (process.env.DEBUG) { console.log('5') }

            const css = {
                top: 'auto',
                position: 'static',
                transition: ''
            }
            setState(false, css)
            SCENE.offset(el.offsetHeight)
            SCENE.triggerHook(moreArticlesVisible.heightInPercentage)
        }
        //If footer is reached
        else if (pinState && REACHED_BOTTOM ) {
            if (process.env.DEBUG) { console.log('6') }

            const css = {  
                top: r.footerCoordinatesTop() - el.offsetHeight  + 'px',
                position: 'absolute',
                transition: ''
            }
            setState(false, css)
            SCENE.offset(r.footerCoordinatesTop() - el.offsetHeight - r.startingCoordinatesTop())
            SCENE.triggerHook(r.calcTriggerHookFromTop(r.siteNavHeight))
        }
    }

    function distance(startingPos, currentPos, offset) {
        return (currentPos + offset) - startingPos
    }

    function setStylesForElement(el) {
        return function(css) {
            for (var prop in css) {
                if (css.hasOwnProperty(prop)) {
                    el.style[prop] = css[prop]
                } 
            }
        }
    }

    function setPin(bool) {
        pinState = bool
    }

    function setState(setPinVal,setStyles) {
        setPin(setPinVal)
        setStylesForElement(aside)(setStyles)
    }

    function moreArticlesIsVisible() {
        //check if more articles is loaded if not use default values
        const moreArticles = document.querySelector('.more-articles')
        const height =  moreArticles ? Math.abs((moreArticles.getBoundingClientRect().top - w.innerHeight)) : 0 ;

        return {
            height,
            heightInPercentage: (w.innerHeight - height)/w.innerHeight
        }
    }
    
}