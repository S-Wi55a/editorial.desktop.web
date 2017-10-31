import path from 'path'
import webpack from 'webpack'
import {IS_PROD, IS_DEV} from '../Shared/env.config.js'
import ExtractTextPlugin from 'extract-text-webpack-plugin'
import ForkTsCheckerNotifierWebpackPlugin from 'fork-ts-checker-notifier-webpack-plugin'
import WebpackNotifierPlugin from 'webpack-notifier'
import WatchMissingNodeModulesPlugin from 'react-dev-utils/WatchMissingNodeModulesPlugin'

export const plugins = (tenant = 'sever') => {

    let pluginsArr = [
        new webpack.DefinePlugin({
            'process.env.NODE_ENV': IS_PROD ? '"production"': '"development"', //TODO add to shared / Correct the logic
            SERVER: JSON.stringify(true)
        }),
        new ExtractTextPlugin({
            filename: IS_PROD ? '[name]-[contenthash].css' : '[name].css',
            allChunks: false
        }),
        new webpack.NamedModulesPlugin(),
        new webpack.EnvironmentPlugin({
            NODE_ENV: 'development', // use 'development' unless process.env.NODE_ENV is defined
            DEBUG: false
        }),
        new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/)           
    ]

    if (IS_PROD) {
        pluginsArr.push(new webpack.optimize.ModuleConcatenationPlugin())
        pluginsArr.push(
            new webpack.optimize.UglifyJsPlugin({
                compress: {
                  warnings: false,
                  // Disabled because of an issue with Uglify breaking seemingly valid code:
                  // https://github.com/facebookincubator/create-react-app/issues/2376
                  // Pending further investigation:
                  // https://github.com/mishoo/UglifyJS2/issues/2011
                  comparisons: false,
                },
                output: {
                  comments: false,
                  // Turned on because emoji and regex is not minified properly using default
                  // https://github.com/facebookincubator/create-react-app/issues/2488
                  ascii_only: true,
                },
                sourceMap: false,
              })
        )
    }
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