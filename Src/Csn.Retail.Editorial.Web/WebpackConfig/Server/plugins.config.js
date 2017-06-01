import webpack from 'webpack'
import {isProd} from '../Shared/env.config.js'
import {devLoaderCSSExtract} from '../Shared/loaders.config.js'
import ExtractTextPlugin from 'extract-text-webpack-plugin'
import AssetsPlugin from 'assets-webpack-plugin'
import BrowserSyncPlugin from 'browser-sync-webpack-plugin'
import HappyPack from 'happypack'

var assetsPluginInstance = new AssetsPlugin({
    filename: 'webpack.assets.json',
    path: __dirname,
    prettyPrint: true,
    fullPath: false
});

export const plugins = (tenant) => {
    return [
        assetsPluginInstance,
        new ExtractTextPlugin({
            filename: isProd ? '[name]-[contenthash].css' : '[name].css',
            allChunks: false
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
        })
    ]
}