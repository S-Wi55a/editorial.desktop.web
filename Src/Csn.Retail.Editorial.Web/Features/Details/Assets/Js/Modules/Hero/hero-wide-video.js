const watchVideo = document.querySelector('.hero__playbutton');
const heroContent = document.querySelector('.hero__content');
const heroFullVideo = document.querySelector('.fullherovideo--wrap');
const heroFullVideoClose = document.querySelector('.fullherovideo__action-close');
const heroFulVideoContainer = document.querySelector('.fullherovideo__container');

const heroVideoHtml = (videoId, playerId) => {
    return `
        <video id="fullhero_video" playsinline="true" preload="auto" autoplay
                data-video-id="${videoId || ''}"
                data-account="674523943001"
                data-player="${playerId || ''}"
                data-embed="default"
                class="video-js" controls=""></video>
    `;
};

const heroVideoScript = (playerId) => {
    return `
        <script src="//players.brightcove.net/674523943001/${playerId || ''}_default/index.min.js"></script>
    `;
};

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

    let heroVideoId  = watchVideo.getAttribute('data-video-id');
    let heroPlayerId = watchVideo.getAttribute('data-player-id');


    heroFulVideoContainer.innerHTML = heroVideoHtml(heroVideoId, heroPlayerId);

    var s = document.createElement('script');
    s.src = "//players.brightcove.net/674523943001/" + heroPlayerId + "_default/index.min.js";
    document.body.appendChild(s);
    s.onload = heroCallBack;
}

function heroCallBack() {
    myPlayer = videojs('fullhero_video');
    myPlayer.play();
}