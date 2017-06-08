import path from 'path'
import webpack from 'webpack'
import {isProd} from '../Shared/env.config.js'
import ExtractTextPlugin from 'extract-text-webpack-plugin'
import AssetsPlugin from 'assets-webpack-plugin'
import BrowserSyncPlugin from 'browser-sync-webpack-plugin'
import HappyPack from 'happypack'

//From Server/
import {devLoaderCSSExtract} from './loaders.config.js'

var assetsPluginInstance = new AssetsPlugin({
    filename: 'webpack.assets.json',
    path: path.resolve('./'),
    prettyPrint: true,
    fullPath: false,
    update: true
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