import postscribe from 'postscribe';
import { loaded } from 'document-promises/document-promises.js'

loaded.then(() => {
   //Look through all the ad tiles and use postscribe to laod them asynchronously
   (function ($, w) {

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
                $(".mediamotive-block").each(function (index ) {
                    var item = $(this);
                    var tile = item.attr('id');
                    var isKruxRequired = (item.attr('data-krux-required') == 'true');
                    var scriptUrl = item.attr('data-media-motive-url');
                    if (isKruxRequired) {
                        scriptUrl = scriptUrl + sasTags;
                    }
                    postscribe('#' + tile, '<script src="' + scriptUrl + '"><\/script>', {
                        done: function () {
                            $('#' + tile).removeClass('mediamotive-block--loading');

                            if (index === $(".mediamotive-block").length - 1) {
                                window.csnKruxLoader()
                            }
                        }
                    });

                });

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
});


    
 
