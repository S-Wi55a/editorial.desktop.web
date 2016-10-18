//Plugin start
(function ($) {
    "use strict";
    var namespace = "webmclicktracking",

        events = {
            click: "click." + namespace
        },

        defaultOptions = {
            pageTag: 'WT.z_pg',
            sectionTag: 'WT.nv',
            clickTag: 'WT.z_ti',
            adclickTag: 'WT.ac'
        },

        trackClick = function (section, click, page, adClick) {
            if (section && click && (typeof (dcsMultiTrack) == 'function')) {
                var secVal = section.value || 'Page Level';
                var pageVal = page.value || document.location.pathname;
                if (!adClick) {
                    dcsMultiTrack('WT.dl', '99', section.tag, secVal, click.tag, click.value, page.tag, pageVal);
                } else {
                    dcsMultiTrack('WT.dl', '99', section.tag, secVal, click.tag, click.value, page.tag, pageVal, adClick.tag, adClick.value);
                }
            }
        };

    /***** Plugin Methods *****/
    var methods = {
        init: function (options) {
            this.each(function () {
                var $this = $(this);
                var opts = $.extend({}, defaultOptions, options);
                $this.data(namespace, opts);
            });

            return this;
        },

        track: function (sectionValue, clickValue, pageValue, advalue) {
            var element = $(this),
                opts = $(this).data(namespace) || defaultOptions,
                section = {
                    tag: opts.sectionTag,
                    value: sectionValue
                },
                click = {
                    tag: opts.clickTag,
                    value: clickValue
                },
                page = {
                    tag: opts.pageTag,
                    value: pageValue
                };

            var adClick;
            if (advalue) {
                adClick = {
                    tag: opts.adclickTag,
                    value: advalue
                };
            }
            trackClick(section, click, page, adClick);
        },

        destroy: function () {
            this.each(function () {
                $(this).off("." + namespace);
                $(this).data(namespace, null);
            });
        }
    };

    /***** Plugin Entry *****/
    $.fn.webmClickTracking = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            return null;
        }
    };

    //Auto wireup
    $(function () {
        $(document).on(events.click, '[data-webm-clickvalue]', function (e) {
            var element = $(this),
                value = element.data('webm-clickvalue'),
                advalue = element.data('webm-adclickvalue'),
                section = element.parents('[data-webm-section]:first').data('webm-section'),
                page = element.parents('[data-webm-page]:first').data('webm-page');
            element.webmClickTracking("track", section, value, page, advalue);
        });
    });
})(jQuery);
//Plugin end