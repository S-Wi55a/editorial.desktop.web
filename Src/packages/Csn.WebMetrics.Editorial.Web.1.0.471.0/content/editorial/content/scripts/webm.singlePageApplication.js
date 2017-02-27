(function () {
    function WebmSpaTracker(baseTags) {
        
        var currentTags = [];
        var submit = function(arr) {
            if (typeof(dcsMultiTrack) === "function") {
                dcsMultiTrack.apply(null, arr);
            }
        };
        var trackEvent = function(argsarr) {
            for (var k in currentTags) {
                argsarr.push(currentTags[k].name);
                argsarr.push(currentTags[k].value);
            }
            submit(argsarr);
        };
        
        if (Array.isArray(baseTags)) {
            currentTags = currentTags.concat(baseTags);
        }

        var cloneTags = function () {
            return currentTags.slice(0);
        }

        return {
            currentTags: currentTags,
            trackPageView: function () {
                var argsarr = ["WT.dl", "0", "DCS.dcsuri", document.location.pathname];
                trackEvent(argsarr);
                return this;
            },
            trackEvent: function (eventName) {
                var argsarr = ["WT.dl", "99", "DCS.dcsuri", document.location.pathname, "WT.conv", eventName];
                trackEvent(argsarr);
                return this;
            },
            addTag: function (tName, tValue) {
                var newArr = cloneTags();
                newArr.push({ name: tName, value: tValue });
                return new WebmSpaTracker(newArr);
            }
        }
    };
    window.webmSpaTracker = window.webmSpaTracker || new WebmSpaTracker();
})();


