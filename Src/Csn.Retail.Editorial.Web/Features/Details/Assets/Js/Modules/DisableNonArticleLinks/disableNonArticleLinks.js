const disableNonArticleLinks = (scope) => {
    [...scope.getElementsByTagName('a')].forEach((element) => {
        let articleUrl = element.getAttribute('href');
        if(articleUrl) {
            if(csn_editorial.detailsModal && csn_editorial.detailsModal.pathRegex.test(articleUrl)) {
                element.style.cursor = 'pointer';
                element.onclick = () => window.location.replace(csn_editorial.detailsModal.pathRedirect + articleUrl.split('-').pop(), '_self');
            }
            else {
                element.style.pointerEvents = 'none';
            }
            element.removeAttribute('href');
        }
    });
}

export default disableNonArticleLinks;