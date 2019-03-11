import path from 'path'
import webpack from 'webpack'
import {IS_PROD, IS_DEV} from '../Shared/env.config.js'
import MiniCssExtractPlugin from 'mini-css-extract-plugin'
import ForkTsCheckerNotifierWebpackPlugin from 'fork-ts-checker-notifier-webpack-plugin'
import WebpackNotifierPlugin from 'webpack-notifier'
import WatchMissingNodeModulesPlugin from 'react-dev-utils/WatchMissingNodeModulesPlugin'

export const plugins = (tenant = 'sever') => {

    let pluginsArr = [
        new webpack.DefinePlugin({
            SERVER: JSON.stringify(true)
        }),
        new MiniCssExtractPlugin({
            filename: IS_PROD ? '[name]-[contenthash].css' : '[name].css',
            allChunks: false
        }),
        new webpack.EnvironmentPlugin({
            NODE_ENV: 'development', // use 'development' unless process.env.NODE_ENV is defined
            DEBUG: false
        }),
        new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/)           
    ]

    if (IS_DEV) {
        pluginsArr.push(    
            new WatchMissingNodeModulesPlugin(path.resolve('node_modules'))
        )
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