'use strict';

var _envConfig = require('../Shared/env.config.js');

var _tenantsConfig = require('../Shared/tenants.config.js');

var _resolveConfig = require('../Shared/resolve.config.js');

var _loadersConfig = require('../Shared/loaders.config.js');

var _statsConfig = require('../Shared/stats.config.js');

var _devServerConfig = require('../Shared/devServer.config.js');

var _rimraf = require('rimraf');

var _rimraf2 = _interopRequireDefault(_rimraf);

var _entriesConfig = require('../Server/entries.config.js');

var _pluginsConfig = require('../Server/plugins.config.js');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

// Remove dist folder


//From Server/
(0, _rimraf2.default)('./dist--server', function (err) {
    if (err) {
        throw err;
    }
});

module.exports = function () {

    var moduleExportArr = [];

    //run through list of tenants
    _tenantsConfig.TENANTS.forEach(function (tenant) {

        var entries = {};

        entries['react-server-components' + '--' + tenant] = ['./Features/ReactServerRender/Assets/Js/react-server-components.js'];

        moduleExportArr.push({
            target: 'node',
            name: tenant,
            entry: entries,
            output: {
                path: _entriesConfig.config.outputPath,
                publicPath: _entriesConfig.config.publicPath,
                filename: _envConfig.isProd ? '[name]-[chunkhash].js' : '[name].js'
            },
            module: (0, _loadersConfig.modules)(tenant),
            resolve: _resolveConfig.resolve,
            plugins: (0, _pluginsConfig.plugins)(tenant),
            stats: _statsConfig.stats,
            devtool: _envConfig.isProd ? "cheap-source-map" : "eval",
            devServer: (0, _devServerConfig.devServer)(tenant)
        });
    });
    return moduleExportArr;
};