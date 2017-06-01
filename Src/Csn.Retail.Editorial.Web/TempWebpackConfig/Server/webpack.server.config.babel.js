'use strict';

var _envConfig = require('../Shared/env.config.js');

var _tenantsConfig = require('../Shared/tenants.config.js');

var _pathsConfig = require('../Shared/paths.config.js');

var _entriesConfig = require('../Shared/entries.config.js');

var _loadersConfig = require('../Shared/loaders.config.js');

var _pluginsConfig = require('../Shared/plugins.config.js');

var _statsConfig = require('../Shared/stats.config.js');

var _devServerConfig = require('../Shared/devServer.config.js');

var _path = require('path');

var _path2 = _interopRequireDefault(_path);

var _rimraf = require('rimraf');

var _rimraf2 = _interopRequireDefault(_rimraf);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

// Remove dist folder
(0, _rimraf2.default)('./dist', function (err) {
    if (err) {
        throw err;
    }
});

module.exports = function (env) {

    process.env.BABEL_ENV = env;

    var moduleExportArr = [];

    //run through list of tenants
    _tenantsConfig.TENANTS.forEach(function (tenant) {

        var entries = (0, _entriesConfig.getEntryFiles)(tenant);

        var pageEntries = Object.keys((0, _entriesConfig.getEntryFiles)(tenant));

        entries['vendor' + '--' + tenant] = ['./Features/Shared/Assets/Js/vendor.js'];
        entries['csn.base' + '--' + tenant] = ['./Features/Shared/Assets/csn.base.js'];
        entries['csn.mm' + '--' + tenant] = ['./Features/Shared/Assets/Js/Modules/MediaMotive/mm.js'];

        entries['react-server-components' + '--' + tenant] = ['./Features/ReactServerRender/Assets/Js/react-server-components.js'];

        moduleExportArr.push({
            name: tenant,
            entry: entries,
            output: {
                path: _entriesConfig.config.outputPath,
                publicPath: _entriesConfig.config.publicPath,
                filename: _envConfig.isProd ? '[name]-[chunkhash].js' : '[name].js'
            },
            module: (0, _loadersConfig.modules)(tenant),
            resolve: {
                extensions: ['.js', '.scss'],
                alias: {
                    modernizr$: _path2.default.resolve('./.modernizrrc.js'),
                    'debug.addIndicators': _path2.default.resolve('node_modules', 'scrollmagic/scrollmagic/uncompressed/plugins/debug.addIndicators.js')
                },
                //aliasFields: ["browser"],
                descriptionFiles: ['package.json', 'bower.json'],
                modules: _pathsConfig.listOfPaths
            },
            plugins: (0, _pluginsConfig.plugins)(tenant, pageEntries),
            stats: _statsConfig.stats,
            devtool: _envConfig.isProd ? "cheap-source-map" : "eval",
            devServer: (0, _devServerConfig.devServer)(tenant)
        });
    });
    return moduleExportArr;
};