let watchVideo = document.querySelector('.hero__playbutton');
let heroContent = document.querySelector('.hero__content');
let heroFullVideo = document.querySelector('.fullherovideo--wrap');
let heroFullVideoClose = document.querySelector('.fullherovideo__action-close');

if (watchVideo) {
    watchVideo.addEventListener('click',
        (e) => {
            heroContent.classList.toggle('hero__fade-out');
            setTimeout(function () { showHeroVideo(); }, 600);
        }
    );
}

if (heroFullVideoClose) {
    heroFullVideoClose.addEventListener('click',
        (e) => {
            heroContent.classList.remove('hero__fade-out');
            setTimeout(function () { hideHeroVideo(); }, 600);
        }
    );
}

function hideHeroVideo() {
    heroFullVideo.classList.add('fullherovideo--wrap-hide');
    heroFullVideo.classList.remove('fullherovideo--wrap-show');    
}

function showHeroVideo() {
    heroFullVideo.classList.remove('fullherovideo--wrap-hide');
    heroFullVideo.classList.add('fullherovideo--wrap-show');    
}