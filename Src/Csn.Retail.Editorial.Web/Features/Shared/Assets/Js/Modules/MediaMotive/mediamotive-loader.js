﻿(function ($, w, krux) {
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
                    var isKruxRequired = (item.data('kruxRequired') == 'True');
                    var scriptUrl = item.data('mediaMotiveUrl');
                    if (isKruxRequired) {
                        scriptUrl = scriptUrl + sasTags;
                    }
                    postscribe('#' + tile, '<script src=\'' + scriptUrl + '\'><\/script>', function () {
                        $('#' + tile).removeClass('loading');
                    });
                });
            }
        };

        return {
            init: init
        };
    };

})(jQuery, window);


(function () {
    new MediaMotiveLoader().init();
})(jQuery, window);