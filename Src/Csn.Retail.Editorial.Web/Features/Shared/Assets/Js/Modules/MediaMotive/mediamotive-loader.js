const postscribe = require("postscribe");

!(function ($, w) {
    'use strict';

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
                $(".mediamotive-block").each(function () {
                    var item = $(this);
                    var tile = item.attr('id');
                    var isKruxRequired = (item.attr('data-krux-required') == 'True');
                    var scriptUrl = item.attr('data-media-motive-url');
                    if (isKruxRequired) {
                        scriptUrl = scriptUrl + sasTags;
                    }
                    postscribe('#' + tile, '<script src=\'' + scriptUrl + '\'><\/script>',
                    {
                        done: function () {
                            console.log('Done ' + tile);
                        }
                    });
                });
            }
        };

        return {
            init: init
        };
    };

})(jQuery, window);


jQuery(document).ready(function () {
    setTimeout(function () { new MediaMotiveLoader().init(); }, 250);
});