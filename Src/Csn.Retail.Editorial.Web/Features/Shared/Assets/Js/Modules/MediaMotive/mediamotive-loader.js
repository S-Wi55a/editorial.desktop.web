import postscribe from "postscribe";

(function ($, w) {
    'use strict';

    var postscribeQueue = []

    w.MediaMotiveLoader = function () {
        var init = function () {
            var sasTags = '';
            if (w.KruxSasHelper) {
                var sasTagsValue = new w.KruxSasHelper().getSasTags();
                sasTags = '/' + sasTagsValue;
            } else {
                if (console) {
                    console.warn('KruxSasHelper not loaded');
                }
            }

            if ($(".mediamotive-block").length > 0) {
                //Postscribe exhibits strange behaviour when called in parallel
                //To prevent this we will build a queue and use callback for the queue
                $(".mediamotive-block").each(function () {
                    var item = $(this);
                    var tile = item.attr('id');
                    var isKruxRequired = (item.attr('data-krux-required') == 'True');
                    var scriptUrl = item.attr('data-media-motive-url');
                    if (isKruxRequired) {
                        scriptUrl = scriptUrl + sasTags;
                    }
                    postscribeQueue.push({ tile: tile, scriptUrl: scriptUrl})
                });

                //Recursively call postscribe
                var p = function (i, postscribeQueue) {

                    if (i < postscribeQueue.length) {
                        postscribe('#' + postscribeQueue[i].tile, '<script src=\'' + postscribeQueue[i].scriptUrl + '\'><\/script>',
                            {
                                done: function () {
                                    i++
                                    return p(i, postscribeQueue)
                                }
                            });  
                    }
     
                }

                p(0, postscribeQueue)
            }
        };

        return {
            init: init
        };
    };


    $(function(){
        new MediaMotiveLoader().init();
    });

})(jQuery, window);


