let watchVideo = document.querySelector('.hero__playbutton');
let heroContent = document.querySelector('.hero__content');

if (watchVideo) {
    watchVideo.addEventListener('click',
        (e) => {
            heroContent.classList.toggle('hero__fade-out');
        }
    );
}