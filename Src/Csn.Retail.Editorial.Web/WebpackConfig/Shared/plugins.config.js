﻿import webpack from 'webpack'
import {isProd} from '../Shared/env.config.js'
import {devLoaderCSSExtract} from '../Shared/loaders.config.js'



var ExtractTextPlugin = require('extract-text-webpack-plugin'),
    AssetsPlugin = require('assets-webpack-plugin'),
    BrowserSyncPlugin = require('browser-sync-webpack-plugin'),
    HappyPack = require('happypack');


var assetsPluginInstance = new AssetsPlugin({
    filename: 'webpack.assets.json',
    path: __dirname,
    prettyPrint: true,
    fullPath: false
});


export const plugins = (tenant, pageEntries) => {
    return [
        assetsPluginInstance,
        new ExtractTextPlugin({
            filename: isProd ? '[name]-[contenthash].css' : '[name].css',
            allChunks: false
        }),
        //Vendor & Manifest - Nothing is added unless manual 
        new webpack.optimize.CommonsChunkPlugin({
            names: ['vendor' + '--' + tenant, 'csn.base' + '--' + tenant],
            minChunks: Infinity
        }),
        //Per page -- pulll chunks from page and make common chunk async load
        new webpack.optimize.CommonsChunkPlugin({
            names: pageEntries,
            children: true,
            async: true,
            minChunks: 2
        }),
        // Common -- pull everything from pages and make global common chunk
        new webpack.optimize.CommonsChunkPlugin({
            name: 'csn.common' + '--' + tenant,
            chunks: pageEntries,
            minChunks: 2
        }),
        new webpack.NamedModulesPlugin(),
        new HappyPack({
            // loaders is the only required parameter:
            id: 'babel',
            loaders: ['babel-loader?cacheDirectory=true']
        }),
        new HappyPack({
            // loaders is the only required parameter:
            id: 'sass',
            loaders: devLoaderCSSExtract(tenant)
        }),
        new BrowserSyncPlugin(
            // BrowserSync options 
            {
                // browse to http://localhost:3000/ during development 
                host: 'localhost',
                port: 3000,
                // proxy the Webpack Dev Server endpoint 
                // through BrowserSync 
                proxy: {
                    target: 'http://localhost:8080',
                    ws: true
                },
                logLevel: "info",
                open: false

            },
            // plugin options 
            {
                // prevent BrowserSync from reloading the page 
                // and let Webpack Dev Server take care of this 
                reload: false
            }
        )
    ]
}