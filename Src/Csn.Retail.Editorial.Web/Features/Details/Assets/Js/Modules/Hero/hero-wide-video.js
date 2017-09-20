const watchVideo = document.querySelector('.hero__playbutton');
const heroContent = document.querySelector('.hero__content');
const heroFullVideo = document.querySelector('.fullherovideo--wrap');
const heroFullVideoClose = document.querySelector('.fullherovideo__action-close');
const heroFulVideoContainer = document.querySelector('.fullherovideo__container');

const heroVideoHtml = (videoId, playerId) => {
    return `
        <video id="fullhero_video" preload="auto"
            data-video-id="${videoId || ''}"
            data-account="674523943001"
            data-player="${playerId || ''}"
            data-embed="default"
            class="video-js" controls=""></video>
    `;
};

if (watchVideo) {
    const heroVideoId = watchVideo.getAttribute('data-video-id');
    const heroPlayerId = watchVideo.getAttribute('data-player-id');

    heroFulVideoContainer.innerHTML = heroVideoHtml(heroVideoId, heroPlayerId);

    const s = document.createElement('script');
    s.src = "//players.brightcove.net/674523943001/" + heroPlayerId + "_default/index.min.js";
    document.body.appendChild(s);

    watchVideo.addEventListener('click',
        () => {
            playHeroVideo(window.videojs);

            heroContent.classList.toggle('hero__slide-out');
            heroFullVideo.classList.remove('fullherovideo--wrap-hide');
            heroFullVideo.classList.add('fullherovideo--wrap-show');
        }
    );
}

if (heroFullVideoClose) {
    heroFullVideoClose.addEventListener('click',
        () => {
            heroContent.classList.remove('hero__slide-out');
            closeHeroVideo();
        }
    );
}

function closeHeroVideo() {
    stopHeroVideo(window.videojs);
    heroFullVideo.classList.add('fullherovideo--wrap-hide');
    heroFullVideo.classList.remove('fullherovideo--wrap-show');
}

function playHeroVideo(videojs) {
    const myPlayer = videojs('fullhero_video');
    myPlayer.play();
}


function stopHeroVideo(videojs) {
    const myPlayer = videojs('fullhero_video');
    myPlayer.pause();    
}

