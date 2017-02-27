(function () {
    function WebmEventTracker() {
        var plugins = [];
        var installedPlugins = [];

        var methods = {
            trackClick: function (args) {
                for (var i in plugins) {
                    try {
                        plugins[i].trackClick(args);
                    } catch (e) {
                    }
                }
            }
        };

        return {
            addPlugin: function (plugin) {
                if (installedPlugins.indexOf(plugin.key) === -1) {
                    plugins.push(plugin);
                    installedPlugins.push(plugin.key);
                }
            },

            trackClick: function (args) {
                methods.trackClick(args);
            }
        };
    }

    window.webmEventTracker = window.webmEventTracker || new WebmEventTracker();
})();