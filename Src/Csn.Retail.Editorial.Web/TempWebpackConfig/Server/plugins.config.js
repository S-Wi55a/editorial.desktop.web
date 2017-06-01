'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.plugins = undefined;

var _path = require('path');

var _path2 = _interopRequireDefault(_path);

var _webpack = require('webpack');

var _webpack2 = _interopRequireDefault(_webpack);

var _envConfig = require('../Shared/env.config.js');

var _loadersConfig = require('../Shared/loaders.config.js');

var _extractTextWebpackPlugin = require('extract-text-webpack-plugin');

var _extractTextWebpackPlugin2 = _interopRequireDefault(_extractTextWebpackPlugin);

var _assetsWebpackPlugin = require('assets-webpack-plugin');

var _assetsWebpackPlugin2 = _interopRequireDefault(_assetsWebpackPlugin);

var _happypack = require('happypack');

var _happypack2 = _interopRequireDefault(_happypack);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var assetsPluginInstance = new _assetsWebpackPlugin2.default({
    filename: 'webpack.assets.json',
    path: _path2.default.resolve('./'),
    prettyPrint: true,
    fullPath: false,
    update: true
});

var plugins = exports.plugins = function plugins(tenant) {
    return [assetsPluginInstance, new _extractTextWebpackPlugin2.default({
        filename: _envConfig.isProd ? '[name]-[contenthash].css' : '[name].css',
        allChunks: false
    }), new _webpack2.default.NamedModulesPlugin(), new _happypack2.default({
        // loaders is the only required parameter:
        id: 'babel',
        loaders: ['babel-loader?cacheDirectory=true']
    }), new _happypack2.default({
        // loaders is the only required parameter:
        id: 'sass',
        loaders: (0, _loadersConfig.devLoaderCSSExtract)(tenant)
    })];
};