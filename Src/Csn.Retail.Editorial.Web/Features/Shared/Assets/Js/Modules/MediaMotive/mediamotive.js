var MediaMotive = (function ($) {
    "use strict";
    var history = [],
        pageScanned = false,
		scanPage = function () {
		    $("div[id^=Tile]:has(script),div[id^=tempTile]:not([data-is-hoisted])").each(function () {
		        var div = $(this), id = div.attr("id").replace("temp", ""), src = div.children("script:first").attr("src");
		        history.push({ id: id, src: src, isHoisted: false });
		    });
		},
		sortHistory = function (a, b) {
		    // strip the ID of text just to get the number to compare
		    var c = parseInt(a.id.replace(/\D+/, ""), 10),
				d = parseInt(b.id.replace(/\D+/, ""), 10);
		    return c > d ? 1 : (c < d ? -1 : 0);
		};
    return {
        getAds: function () {
            var i;
            if (!pageScanned) {
                scanPage();
                history.sort(sortHistory);
                pageScanned = true;
            }
            for (i = history.length - 1; i >= 0; i -= 1) {
                if (!history[i].container) {
                    history[i].container = $('#' + history[i].id);
                }
            }
            return history;
        },
        getAdContent: function (id) {
            var tempTile = $("#temp" + id);
            var srcChildren = tempTile.children("script:first");
            var src = srcChildren.attr("src");
            var elements;

            if (src && src.indexOf(";sz=1x1") === -1) {
                elements = tempTile.children(":not(script)").find("script").remove().end();

                //  Only hoist ads if a placeholder has not been served
                if (!elements.children("img[src$='grey.gif']").length) {
                    return elements;
                }
            }
            return null;
        },
        hoist: function (id) {
            var content = this.getAdContent(id);
            if (content != null) {
                $("#" + id).empty().append(content).removeClass('csn-doubleclick-preload');
            } else {
                $("#" + id).remove();
            }
        }
    };
}(jQuery));