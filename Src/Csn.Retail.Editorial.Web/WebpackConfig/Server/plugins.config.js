import webpack from 'webpack'
import {isProd} from '../Shared/env.config.js'
import ExtractTextPlugin from 'extract-text-webpack-plugin'
import HappyPack from 'happypack'

//From Server/
import {devLoaderCSSExtract} from './loaders.config.js'

export const plugins = (tenant) => {
    return [
        new webpack.DefinePlugin({
            'process.env.NODE_ENV': '"production"', //TODO add to shared / Correct the logic
            SERVER: JSON.stringify(true)
        }),
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