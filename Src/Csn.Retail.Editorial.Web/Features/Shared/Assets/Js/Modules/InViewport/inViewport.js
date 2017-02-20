export function inViewport(d, frame, selector, threshold = 1, cb) {
    const sections = d.querySelectorAll(selector);

    frame.addEventListener('scroll', inView.bind(null, d, frame, selector, sections, threshold, cb))
    inView(d, frame, selector, sections, threshold, cb);
}
function inView(d, frame, selector, sections, threshold, cb) {

    // Don't run the rest of the code if every section is already visible
    if (d.querySelectorAll(selector + ':not(.visible)').length === 0) return;


    // Run this code for every section in sections
    for (const section of sections) {
        if (section.getBoundingClientRect().top <= frame.innerHeight * threshold && section.getBoundingClientRect().top > 0) {
            section.classList.add('visible');
            cb()
            frame.removeEventListener('scroll', inView)
        }
    }
}