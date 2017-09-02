//import os from 'os'
import webpack from 'webpack'
import {isProd} from '../Shared/env.config.js'
import ExtractTextPlugin from 'extract-text-webpack-plugin'
// import HappyPack from 'happypack'
// import ForkTsCheckerWebpackPlugin from 'fork-ts-checker-webpack-plugin'
import ForkTsCheckerNotifierWebpackPlugin from 'fork-ts-checker-notifier-webpack-plugin'
import WebpackNotifierPlugin from 'webpack-notifier'

export const plugins = (tenant = 'sever') => {

    let pluginsArr = [
        new webpack.DefinePlugin({
            'process.env.NODE_ENV': isProd ? '"production"': '"development"', //TODO add to shared / Correct the logic
            SERVER: JSON.stringify(true)
        }),
        new ExtractTextPlugin({
            filename: isProd ? '[name]-[contenthash].css' : '[name].css',
            allChunks: false
        }),
        new webpack.NamedModulesPlugin(),
        new webpack.EnvironmentPlugin({
            NODE_ENV: 'development', // use 'development' unless process.env.NODE_ENV is defined
            DEBUG: false
        })
    ]

    if (isProd) {
        pluginsArr.push(new webpack.optimize.ModuleConcatenationPlugin())
    }
    if (!isProd) {
        pluginsArr.push(
            new WebpackNotifierPlugin({ 
                title: `${tenant} - Server - Webpack`,
            })
        )
        pluginsArr.push(
            new ForkTsCheckerNotifierWebpackPlugin({ 
                title: `${tenant} - Server - TypeScript`,
            })
        )       
    }        
    return pluginsArr
}