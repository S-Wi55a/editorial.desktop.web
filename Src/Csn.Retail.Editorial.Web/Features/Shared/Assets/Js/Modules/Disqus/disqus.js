require('Disqus/_disqus.scss')

export default function () {  // REQUIRED CONFIGURATION VARIABLE: EDIT THE SHORTNAME BELOW
    var d = document, s = d.createElement('script');
    var disqus_shortname = window.disqus_shortname;

    s.src = '//' + disqus_shortname + '.disqus.com/embed.js';  // IMPORTANT: Replace EXAMPLE with your forum shortname!
    s.async = true;

    s.setAttribute('data-timestamp', +new Date());
    (d.head || d.body).appendChild(s);
};