//Plugin start
(function ($) {
    "use strict";
    var namespace = "webmhelpmechoosetracking",
        events = {
            click: "click." + namespace,
        },
        defaultOptions = {
            actionTag: 'WT.z_svaction',
            nonSponsoredTag: 'WT.z_nonSponsoredVehicle',
            sponsoredTag: 'WT.z_sponsoredVehicle',
            questionTag: 'WT.z_question',
            answerTag: 'WT.ti',
            decode: false
        },
        queryBuilder = function (url, noDecode) {
            var list = {},
                queryString = (url.match(/\?([^#]+)#?/) ? RegExp.$1 : ""),
                params = queryString.split("&");

            $.each(params, function () {
                var keyValue = this.split("=");
                list[keyValue[0]] = !noDecode ? decodeURI(keyValue[1]) : keyValue[1];
            });

            return {
                update: function (key, value) {
                    list[key] = value;
                },
                remove: function (key) {
                    delete list[key];
                },
                value: function (key) {
                    return list[key] || "";
                },
                toString: function () {
                    var result = [];
                    for (var key in list) {
                        if (key && list.hasOwnProperty(key)) {
                            result.push(key + "=" + $.trim(list[key]));
                        }
                    }
                    return result.join("&");
                }
            };
        },
        appendParams = function (anchor, queryParams, options) {
            var uriComponent = anchor.protocol + "//" + anchor.host + anchor.pathname,
                queryStringComponent;

            var qb = queryBuilder(anchor.href, options.decode);
            $.each(queryParams, function (key, value) {
                qb.update(key, encodeURIComponent(value));
            });
            queryStringComponent = qb.toString();

            return uriComponent + '?' + queryStringComponent + anchor.hash;
        },
        trackClick = function (event, anchor, tags, options) {
            if (event && anchor && tags && options) {
                try {
                    var linkUrl = appendParams(anchor, tags, options);
                    event.preventDefault();
                    window.location = linkUrl;
                } catch (e) {
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

        trackQuestion: function (event, question, answer) {
            var element = $(this),
                opts = element.data(namespace) || defaultOptions,
                tags = {};

            tags[opts.questionTag] = question;
            tags[opts.answerTag] = answer;

            trackClick(event, element.get(0), tags, opts);
        },
        
        trackSponsored: function (event, title) {
            var element = $(this),
                opts = element.data(namespace) || defaultOptions,
                tags = {};

            tags[opts.sponsoredTag] = title;

            trackClick(event, element.get(0), tags, opts);
        },
        
        trackNonSponsored: function (event, title) {
            var element = $(this),
                 opts = element.data(namespace) || defaultOptions,
                 tags = {};

            tags[opts.nonSponsoredTag] = title;

            trackClick(event, element.get(0), tags, opts);
        },

        destroy: function () {
            this.each(function () {
                $(this).off("." + namespace);
                $(this).data(namespace, null);
            });
        }
    };

    /***** Plugin Entry *****/
    $.fn.webmHelpMeChooseTracking = function (method) {
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
        $(document).on(events.click, '[data-webm-hmc-answer]', function (e) {
            var element = $(this),
                answer = element.data('webm-hmc-answer'),
                question = element.parents('[data-webm-hmc-question]:first').data('webm-hmc-question');
            element.webmHelpMeChooseTracking("trackQuestion", e, question, answer);
        });
        $(document).on(events.click, '[data-webm-hmc-sponsored]', function (e) {
            var element = $(this),
                title = element.data('webm-hmc-sponsored');
            element.webmHelpMeChooseTracking("trackSponsored", e, title);
        });
        $(document).on(events.click, '[data-webm-hmc-nonsponsored]', function (e) {
            var element = $(this),
                title = element.data('webm-hmc-nonsponsored');
            element.webmHelpMeChooseTracking("trackNonSponsored", e, title);
        });
    });
})(jQuery);
//Plugin end