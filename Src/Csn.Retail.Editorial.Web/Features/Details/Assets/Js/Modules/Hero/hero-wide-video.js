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
    let heroVideoId = watchVideo.getAttribute('data-video-id');
    let heroPlayerId = watchVideo.getAttribute('data-player-id');

    heroFulVideoContainer.innerHTML = heroVideoHtml(heroVideoId, heroPlayerId);

    let s = document.createElement('script');
    s.src = "//players.brightcove.net/674523943001/" + heroPlayerId + "_default/index.min.js";
    document.body.appendChild(s);

    watchVideo.addEventListener('click',
        (e) => {
            playHeroVideo();

            setTimeout(function () {
                heroContent.classList.toggle('hero__fade-out');
                heroFullVideo.classList.remove('fullherovideo--wrap-hide');
                heroFullVideo.classList.add('fullherovideo--wrap-show');
            }, 100);
        }
    );
}

if (heroFullVideoClose) {
    heroFullVideoClose.addEventListener('click',
        (e) => {
            heroContent.classList.remove('hero__fade-out');
            setTimeout(function () { closeHeroVideo(); }, 600);
        }
    );
}

function closeHeroVideo() {
    stopHeroVideo();
    heroFullVideo.classList.add('fullherovideo--wrap-hide');
    heroFullVideo.classList.remove('fullherovideo--wrap-show');
}

function playHeroVideo() {
    let myPlayer = videojs('fullhero_video');
    myPlayer.play();
}


function stopHeroVideo() {
    let myPlayer = videojs('fullhero_video');
    myPlayer.pause();    
}

