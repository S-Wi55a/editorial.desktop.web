'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.modules = exports.devLoaderCSSExtract = exports.prodLoaderCSSExtract = undefined;

var _envConfig = require('../Shared/env.config.js');

var _pathsConfig = require('../Shared/paths.config.js');

var path = require('path');

var ExtractTextPlugin = require('extract-text-webpack-plugin');

// Error with sourcemaps b/c of css-loader. So inline URL to resolve issue (for development only)
var URL_LIMIT = _envConfig.isProd ? 1 : null;

var loaders = function loaders(tenant) {
    return [{
        loader: 'css-loader',
        options: {
            sourceMap: _envConfig.isProd ? false : true,
            minimize: _envConfig.isProd ? true : false
        }
    }, 'postcss-loader?sourceMap', {
        loader: 'resolve-url-loader',
        options: {
            sourceMap: _envConfig.isProd ? false : true,
            keepQuery: true
        }
    }, {
        loader: 'sass-loader',
        options: {
            includePaths: _pathsConfig.listOfPaths,
            sourceMap: true,
            data: '@import "Css/Tenants/' + tenant + '/' + tenant + '.scss";'
        }
    }];
};

var prodLoaderCSSExtract = exports.prodLoaderCSSExtract = function prodLoaderCSSExtract(tenant) {
    return ExtractTextPlugin.extract({
        fallback: 'style-loader',
        use: loaders(tenant)
    });
};

var devLoaderCSSExtract = exports.devLoaderCSSExtract = function devLoaderCSSExtract(tenant) {
    return ['style-loader'].concat(loaders(tenant));
};

var modules = exports.modules = function modules(tenant) {

    return {
        noParse: _envConfig.isProd ? /\A(?!x)x/ : /jquery|swiper|ScrollMagic|modernizr|TinyAnimate|circles/,
        rules: [{
            test: require.resolve(path.resolve('Features/ReactServerRender/Assets/Js/index.js')),
            use: [{
                loader: 'expose-loader',
                options: 'ReactServerComponents'
            }]
        }, {
            test: [/\.js$/, /\.es6$/],
            exclude: /(node_modules|bower_components|unitTest)/,
            loaders: ['happypack/loader?id=babel']
        }, {
            test: /\.modernizrrc.js$/,
            exclude: /(node_modules|bower_components|unitTest)/,
            loader: "modernizr-loader"
        }, {
            test: /\.css$/,
            exclude: /(node_modules|bower_components|unitTest)/,
            use: _envConfig.isProd ? prodLoaderCSSExtract(tenant) : devLoaderCSSExtract(tenant)

        }, {
            test: /\.scss$/,
            exclude: [/(node_modules|bower_components|unitTest)/],
            use: _envConfig.isProd ? prodLoaderCSSExtract(tenant) : devLoaderCSSExtract(tenant)
        }, {
            test: /.*\.(gif|png|jpe?g|svg)$/i,
            exclude: [/(node_modules|bower_components|unitTest)/, /fonts/],
            use: [{
                loader: 'url-loader',
                options: {
                    limit: URL_LIMIT,
                    name: _envConfig.isProd ? 'images/[name]-[hash].[ext]' : 'images/[name].[ext]'
                }
            }, {
                loader: 'image-webpack-loader',
                options: {
                    progressive: true,
                    optipng: {
                        optimizationLevel: 7
                    },
                    gifsicle: {
                        interlaced: false
                    },
                    pngquant: {
                        quality: '65-90',
                        speed: 4
                    }
                }
            }]
        }, {
            test: /\.(eot|svg|ttf|woff|woff2)$/,
            exclude: /(images|img)/,
            use: [{
                loader: 'url-loader',
                options: {
                    limit: URL_LIMIT,
                    name: _envConfig.isProd ? 'fonts/[name]-[hash].[ext]' : 'fonts/[name].[ext]'
                }
            }]
        }]
    };
};